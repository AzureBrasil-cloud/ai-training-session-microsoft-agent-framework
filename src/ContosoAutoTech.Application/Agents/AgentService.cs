using ContosoAutoTech.Application.Tools;
using ContosoAutoTech.Data;
using ContosoAutoTech.Data.Entities;
using ContosoAutoTech.Infrastructure.AIAgent;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService(
    AppDbContext context,
    IConfiguration configuration,
    ILogger<AgentService> logger,
    AiAgentService aiAgentService)
{
    private Credentials GetCredentials()
    {
        var foundryEndpoint = configuration["AiFoundry:Endpoint"]!;
        var foudryApiKey = configuration["AiFoundry:ApiKey"]!;
        var model = configuration["AiFoundry:ChatModel"]!;

        return new Credentials(foundryEndpoint, foudryApiKey, model);
    }
    
    private IList<AITool>? GetToolsByFeature(Feature requestFeature)
    {
        var tools = new List<AITool>();
        
        switch (requestFeature)
        {
            case Feature.CarRegistration:
                var carTools = new CarTools(context);
                
                var createCarTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.CreateCar))!,
                    carTools);

                var updateCarTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.UpdateCar))!,
                    carTools);

                var listCarsTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.ListCars))!,
                    carTools);

                var getCarTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.GetCarById))!,
                    carTools);

                var deleteCarTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.DeleteCar))!,
                    carTools);
                
                tools.Add(createCarTool);
                tools.Add(updateCarTool);
                tools.Add(listCarsTool);
                tools.Add(getCarTool);
                tools.Add(deleteCarTool);
                
                break;
        }
        
        return tools;
    }
};
