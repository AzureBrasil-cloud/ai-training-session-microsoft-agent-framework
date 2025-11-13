using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using ContosoAutoTech.Data;
using ContosoAutoTech.Data.Entities;
using ContosoAutoTech.Infrastructure.AiInference;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

namespace ContosoAutoTech.Application.Tools;

public class CarSalesTool
{
    private const string FeaturesInstructions = "Your are an expert automotive analyst. Your task is to analyze car descriptions and extract key strengths and weaknesses based on the provided details.";
    private const string AgentConsiderationsInstructions = @"You are a sales agent assistant for automotive sales.

Your task is to analyze the car's pricing and provide a brief consideration for the sales agent.

**Chain of Thought Process:**
1. First, check if a reference price is available
2. If yes, compare the actual price with the reference price to determine if it's fair, expensive, or cheap
3. Analyze the strengths and weaknesses provided
4. Provide your final consideration based on all factors

**Instructions:**
- If reference price IS available: State clearly if the car is 'JUSTO' (fair), 'CARO' (expensive), or 'BARATO' (cheap). Explain why and show your considerations based on pricing and features.
- If reference price is NOT available: Do NOT mention if it's fair, expensive, or cheap. Only provide considerations based on the car's features.
- Keep it concise and actionable (maximum 3-4 sentences).
- Always respond in Portuguese (pt-BR).

**Examples:**

Example 1 (WITH reference price - Fair):
Input:
Pricing: Reference price: R$ 110.000,00, Actual price: R$ 108.900,00
Strengths: Motor potente, interior completo, documentação em dia
Weaknesses: Amassado na lateral, riscos no para-choque

Output:
JUSTO. O preço está 1% abaixo da referência de mercado (R$ 110.000,00), tornando-o competitivo. Os pontos fortes incluem motor potente e interior completo, enquanto os danos estéticos (amassado lateral e riscos) justificam o desconto. Excelente oportunidade para clientes que aceitam reparos cosméticos.

Example 2 (WITH reference price - Expensive):
Input:
Pricing: Reference price: R$ 120.000,00, Actual price: R$ 126.500,00
Strengths: Teto solar, sistema de som premium, nunca envolvido em acidentes
Weaknesses: Desgaste no volante, banco do motorista

Output:
CARO. O preço está 5,4% acima da referência de mercado (R$ 120.000,00). Apesar dos extras premium (teto solar e som), os desgastes internos não justificam o valor elevado. Recomenda-se negociar redução ou destacar fortemente os diferenciais para justificar o investimento adicional.

Example 3 (WITH reference price - Cheap):
Input:
Pricing: Reference price: R$ 110.000,00, Actual price: R$ 95.000,00
Strengths: Todas as revisões em concessionária, pneus novos
Weaknesses: Pintura queimada no retrovisor, não revisado nos últimos 10 mil km

Output:
BARATO. O preço está 13,6% abaixo da referência de mercado (R$ 110.000,00), representando excelente valor. O histórico de manutenção em concessionária e pneus novos compensam amplamente a pintura do retrovisor e a revisão pendente. Forte argumento de venda focado no custo-benefício.

Example 4 (WITHOUT reference price):
Input:
Pricing: Actual price: R$ 112.800,00 (no reference price available)
Strengths: Câmera 360º, controle de tração, bancos aquecidos
Weaknesses: Arranhão na porta, pintura queimada no retrovisor

Output:
O veículo apresenta tecnologias avançadas (câmera 360º, controle de tração e bancos aquecidos) que agregam valor significativo. Os danos cosméticos (arranhão e pintura) são reparáveis e não afetam funcionalidade. Destaque as tecnologias de segurança e conforto durante a negociação.

Now, analyze the following car:";

    
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
            var features = await CreateFeaturesAsync(description);

            if (features == null)
            {
                activity?.SetTag("error.reason", "failed_to_deserialize_features");
                return "Error: Failed to extract features from description";
            }

            var referencePrice = await _context.CarReferencePrices
                .Where(x => x.Model == model)
                .Select(x => x.Price)
                .FirstOrDefaultAsync();

            var agentConsiderations = await CreateAgentConsiderationAsync(referencePrice, price, features);

            var carSale = new CarSale(
                imageUrl: imageUrl,
                model: model,
                licensePlate: licensePlate,
                color: color,
                price: price,
                description: description,
                referencePrice: referencePrice,
                agentConsideration: agentConsiderations,
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

    private async Task<string> CreateAgentConsiderationAsync(decimal referencePrice, decimal price, CarSale.Features features)
    {
        var priceComparison = referencePrice > 0 
            ? $"Reference price: R$ {referencePrice:N2}, Actual price: R$ {price:N2}"
            : $"Actual price: R$ {price:N2} (no reference price available)";

        var strengthsList = features.Strengths.Any()
            ? string.Join(", ", features.Strengths)
            : "None identified";

        var weaknessesList = features.Weaknesses.Any()
            ? string.Join(", ", features.Weaknesses)
            : "None identified";

                    var message = $@"
            Pricing: {priceComparison}
            Strengths: {strengthsList}
            Weaknesses: {weaknessesList}";

        var considerationResult = await _aiInferenceService.CompleteAsync(
            _credentials,
            AgentConsiderationsInstructions,
            message);

        return considerationResult.Text;
    }

    private async Task<CarSale.Features?> CreateFeaturesAsync(string description)
    {
        JsonElement schema = AIJsonUtilities.CreateJsonSchema(typeof(CarSale.Features));
            
        ChatOptions chatOptions = new()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema(
                schema: schema,
                schemaName: "DescriptionFeatures",
                schemaDescription: "Extracted strengths and weaknesses of the car from the description")
        };
            
        var featureInferenceResult = await _aiInferenceService.CompleteAsync(
            _credentials,
            FeaturesInstructions,
            description,
            chatOptions: chatOptions);

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<CarSale.Features>(featureInferenceResult.Text, jsonOptions);
    }
}

