using System.Diagnostics;
using ContosoAutoTech.Application;
using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Data.Entities;

namespace ContosoAutoTech.Web.HostedServices;

public class CarSalesHostedService(
    IServiceProvider serviceProvider,
    ILogger<CarSalesHostedService> logger,
    IConfiguration configuration) : BackgroundService
{
    private static readonly ActivitySource ActivitySource = InstrumentationConfig.ActivitySource;
    
    private const string Instructions = """
                                        You are a car sales agent for Contoso AutoTech.
                                        
                                        Your task is to:
                                        1. Get all available cars for sale from the remote sales system using GetAvailableCarsForSale
                                        2. Parse the HTML to extract information for each car
                                        3. For EACH car found, call ProcessCarInformation with the following parameters:
                                           - imageUrl: The URL of the car image
                                           - model: The full model name (e.g., "Toyota Corolla XEi 2.0")
                                           - color: The color of the car
                                           - licensePlate: The license plate number
                                           - price: The numeric price value (extract only the number, without R$ or commas)
                                           - description: The full description text
                                        4. After processing all cars successfully, output ONLY: OK
                                           If any error occurs during processing, output only: ERROR: <error message>
                                        
                                        IMPORTANT: 
                                        - You must call ProcessCarInformation once for each car you find in the HTML.
                                        - Extract the price as a decimal number (e.g., 108900.00 instead of "R$ 108.900,00").
                                        """;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var execute = configuration.GetValue<bool>("Application:ExecuteCarsSalesAgent");
        
        if (!execute)
        {
            logger.LogInformation("CarSalesWebApp agent execution is disabled.");
            return;
        }

        logger.LogInformation("CarSalesWebApp background service started.");

        try
        {
            using var scope = serviceProvider.CreateScope();
            var agentService = scope.ServiceProvider.GetRequiredService<AgentService>();
            
            using Activity? activity = ActivitySource.StartActivity(nameof(CarSalesHostedService));

            var thread = await agentService.CreateThreadAsync(new CreateThreadRequest(Feature.CarSales));

            var result = await agentService.RunAsync(new CreateRunRequest(
                Feature.CarSales,
                "CarSalesWebApp",
                Instructions,
                thread.Value.Id.ToString(),
                "Run it"));

            if (result.IsSuccess)
            {
                logger.LogInformation("Car Sales Agent executed successfully. Result: {Result}", result.Value);
            }
            else
            {
                logger.LogError("Car Sales Agent execution failed. Error: {Error}", result.Error);
            }
            
            logger.LogInformation("CarSalesWebApp agent execution completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during CarSalesWebApp agent execution");
            throw;
        }
    }
}