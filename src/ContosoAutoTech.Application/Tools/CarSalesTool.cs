using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using ContosoAutoTech.Data;
using ContosoAutoTech.Data.Entities;
using ContosoAutoTech.Infrastructure.AIAgent;
using ContosoAutoTech.Infrastructure.AiInference;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

namespace ContosoAutoTech.Application.Tools;

public class CarSalesTool
{
    private static readonly ActivitySource ActivitySource = InstrumentationConfig.ActivitySource;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AiInferenceService _aiInferenceService;
    private readonly AppDbContext _context;
    private readonly Credentials _credentials;
    private readonly string _carSalesRemoteUrl;

    public CarSalesTool(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        AiInferenceService aiInferenceService,
        AppDbContext context,
        Credentials credentials)
    {
        _httpClientFactory = httpClientFactory;
        _aiInferenceService = aiInferenceService;
        _context = context;
        _credentials = credentials;
        _carSalesRemoteUrl = configuration["Application:CarSalesRemoteUrl"] 
                             ?? throw new InvalidOperationException("CarSalesRemoteUrl is not configured");
        _carSalesRemoteUrl = _carSalesRemoteUrl.TrimEnd('/');
    }

    [Description("Get available cars for sale from the remote sales system. Returns raw, unformatted HTML content from the page.")]
    public async Task<string> GetAvailableCarsForSale()
    {
        using Activity? activity = ActivitySource.StartActivity();
        activity?.SetTag("carsales.operation", "get_available_cars");
        activity?.SetTag("carsales.remote_url", _carSalesRemoteUrl);

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(_carSalesRemoteUrl);
            
            activity?.SetTag("http.status_code", (int)response.StatusCode);
            activity?.SetTag("operation.success", response.IsSuccessStatusCode);

            if (!response.IsSuccessStatusCode)
            {
                activity?.SetTag("error.reason", "http_request_failed");
                return $"Error: Failed to retrieve cars for sale. Status code: {response.StatusCode}";
            }

            var content = await response.Content.ReadAsStringAsync();
            
            activity?.SetTag("response.content_length", content.Length);
            
            return content;
        }
        catch (Exception ex)
        {
            activity?.SetTag("operation.success", false);
            activity?.SetTag("error.message", ex.Message);
            activity?.SetTag("error.type", ex.GetType().Name);


            return $"Error retrieving cars for sale: {ex.Message}";
        }
    }

    [Description("Process and save information for a specific car. This method should be called once for each car found in the inventory.")]
    public async Task<string> ProcessCarInformation(
        [Description("The image URL of the car")] string imageUrl,
        [Description("The model of the car (e.g., 'Toyota Corolla XEi 2.0')")] string model,
        [Description("The color of the car")] string color,
        [Description("The license plate of the car")] string licensePlate,
        [Description("The price of the car (numeric value only, without currency symbols)")] decimal price,
        [Description("The full description of the car including features and condition")] string description)
    {
        using Activity? activity = ActivitySource.StartActivity();
        activity?.SetTag("carsales.operation", "process_car_information");
        activity?.SetTag("car.model", model);
        activity?.SetTag("car.color", color);
        activity?.SetTag("car.license_plate", licensePlate);
        activity?.SetTag("car.price", price);

        try
        {

            JsonElement schema = AIJsonUtilities.CreateJsonSchema(typeof(CarSale.Features));
            
            ChatOptions chatOptions = new()
            {
                ResponseFormat = ChatResponseFormat.ForJsonSchema(
                    schema: schema,
                    schemaName: "DescriptionFeatures",
                    schemaDescription: "Extracted strengths and weaknesses of the car from the description")
            };
            
            var inferenceResult = await _aiInferenceService.CompleteAsync(
                _credentials,
                "Your are an expert automotive analyst. Your task is to analyze car descriptions and extract key strengths and weaknesses based on the provided details.",
                description,
                chatOptions: chatOptions);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var features = JsonSerializer.Deserialize<CarSale.Features>(inferenceResult.Text, jsonOptions);

            if (features == null)
            {
                activity?.SetTag("error.reason", "failed_to_deserialize_features");
                return "Error: Failed to extract features from description";
            }

            var carSale = new CarSale(
                model: model,
                licensePlate: licensePlate,
                color: color,
                price: price,
                description: description,
                strengths: features.Strengths,
                weaknesses: features.Weaknesses
            );

            await _context.CarSales.AddAsync(carSale);
            await _context.SaveChangesAsync();
            
            activity?.SetTag("operation.success", true);
            activity?.SetTag("car.id", carSale.Id);

            var result = JsonSerializer.Serialize(new
            {
                success = true,
                message = $"Car '{model}' processed successfully!",
                carId = carSale.Id,
                details = new
                {
                    carSale.Id,
                    carSale.Model,
                    carSale.LicensePlate,
                    carSale.Color,
                    carSale.Price,
                    ImageUrl = imageUrl,
                    carSale.Description,
                    carSale.CarFeatures.Strengths,
                    carSale.CarFeatures.Weaknesses,
                    carSale.CreatedAt
                }
            });

            return result;
        }
        catch (Exception ex)
        {
            activity?.SetTag("operation.success", false);
            activity?.SetTag("error.message", ex.Message);
            activity?.SetTag("error.type", ex.GetType().Name);

            return $"Error processing car information: {ex.Message}";
        }
    }
}

