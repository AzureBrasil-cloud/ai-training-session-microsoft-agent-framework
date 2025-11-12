using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ContosoAutoTech.Application.Tools;

public class CarSalesTool
{
    private static readonly ActivitySource ActivitySource = InstrumentationConfig.ActivitySource;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _carSalesRemoteUrl;

    public CarSalesTool(
        IConfiguration configuration, 
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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
            activity?.SetTag("operation.success", true);

            var result = $"✓ Car processed successfully:\n" +
                        $"  Model: {model}\n" +
                        $"  Color: {color}\n" +
                        $"  License Plate: {licensePlate}\n" +
                        $"  Price: {price:C}\n" +
                        $"  Image: {imageUrl}\n" +
                        $"  Description: {description[..Math.Min(50, description.Length)]}...";

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

