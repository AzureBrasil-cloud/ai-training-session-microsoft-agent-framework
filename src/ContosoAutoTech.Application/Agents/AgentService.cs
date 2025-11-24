using System.Diagnostics;
using ContosoAutoTech.Application.ContextProviders;
using ContosoAutoTech.Application.Tools;
using ContosoAutoTech.Data;
using ContosoAutoTech.Data.Entities;
using ContosoAutoTech.Infrastructure.AIAgent;
using ContosoAutoTech.Infrastructure.AiInference;
using ContosoAutoTech.Infrastructure.Mcps;
using ContosoAutoTech.Infrastructure.AiSearch;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService(
    AppDbContext context,
    IConfiguration configuration,
    ILogger<AgentService> logger,
    McpService mcpService,
    AiAgentService aiAgentService,
    AiSearchService aiSearchService,
    BasicRagService basicRagService,
    IHttpClientFactory httpClientFactory,
    AiInferenceService aiInferenceService)
{
    private static readonly ActivitySource ActivitySource =  InstrumentationConfig.ActivitySource;

    private Credentials GetCredentials()
    {
        var foundryEndpoint = configuration["AiFoundry:Endpoint"]!;
        var foudryApiKey = configuration["AiFoundry:ApiKey"]!;
        var model = configuration["AiFoundry:ChatModel"]!;

        return new Credentials(foundryEndpoint, foudryApiKey, model);
    }

    private async Task<(IList<AITool> tools, IList<McpClient> mcpClients)> GetToolsWithMcpClient(Feature feature)
    {
        var tools = new List<AITool>();
        var mcpClients = new List<McpClient>();
        
        var apimHeader = "Ocp-Apim-Subscription-Key";
        var apiHeaderValue = configuration["Application:SubscriptionMcpKey"];
        switch (feature)
        {
            case Feature.CarPartPrice:
                var (carPartPriceTools, stdioMcpClient) = await mcpService
                    .GetHttpMcpToolAsync("CarPartPriceMcp", configuration["Application:CarPriceMcpRemoteUrl"]!, apimHeader, apiHeaderValue!);
                tools.AddRange(carPartPriceTools);
                mcpClients.Add(stdioMcpClient);
                break;
            case Feature.CarPartStock:
                var (carPartStockTools, httpMcpClient) = await mcpService
                    .GetHttpMcpToolAsync("CarPartStockMcp", configuration["Application:CarStockMcpRemoteUrl"]!, apimHeader, apiHeaderValue!);
                tools.AddRange(carPartStockTools);
                mcpClients.Add(httpMcpClient);
                break;
            case Feature.HumanInLoopClient :
                 var (humanInLoopClientTools, humanInLoopMcpClient) = await mcpService
                     .GetHttpMcpToolAsync("HumanInLoopClientMcp", configuration["Application:CarDiscountMcpRemoteUrl"]!, apimHeader, apiHeaderValue!);
                 var selectedClientTools = humanInLoopClientTools.Where(x => x.Name is  "get_discount_status" or "request_discount");
                 tools.AddRange(selectedClientTools);
                 mcpClients.Add(humanInLoopMcpClient);
                 break;
            case Feature.HumanInLoopManager :
                var (humanInLoopManagerTools, humanInLoopManagerMcpClient) = await mcpService
                    .GetHttpMcpToolAsync("HumanInLoopManagerMcp", configuration["Application:CarDiscountMcpRemoteUrl"]!, apimHeader, apiHeaderValue!);
                var selectedTools = humanInLoopManagerTools.Where(x => x.Name is "decide_approval" or "get_pending_approvals");
                tools.AddRange(selectedTools);
                mcpClients.Add(humanInLoopManagerMcpClient);
                break;
            case Feature.CarPartProduct:
                var headerName = "X-API-KEY";
                var headerKey = "123";
                var (carPartProductTools, carPartProductMcpClient) = await mcpService
                    .GetHttpMcpToolAsync("CarPartProductMcp", configuration["Application:CarProductMcpRemoteUrl"]!, headerName, headerKey);
                tools.AddRange(carPartProductTools);
                mcpClients.Add(carPartProductMcpClient);
                break;
        }

        return (tools, mcpClients);
    }

    private Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider>? GetAiContextProviderByFeature(Feature requestFeature)
    {
        switch (requestFeature)
        {
            case Feature.CustomerRelationsPolicies:

                var useBasicContext = Convert.ToBoolean(configuration["Application:UseBasicCustomerPoliciesAgentContext"]);

                if (useBasicContext)
                {
                        // Option 1: Use mock provider for development/testing (no Azure AI Search required)
                    var basicContextProvider = new BasicRagCustomerPoliciesContextProvider(basicRagService);
                    return basicContextProvider.CreateProviderFactory();
                }

                // Option 2: Use Azure AI Search Knowledge Agent (requires Azure configuration)
                var agenticContextProvider = new AgenticRagCustomerPoliciesContextProvider(aiSearchService, configuration);
                return agenticContextProvider.CreateProviderFactory();
            
            default:
                return null;
        }
    }

    private IList<AITool> GetToolsByFeature(Feature requestFeature)
    {
        var tools = new List<AITool>();

        switch (requestFeature)
        {
            case Feature.CarSales:
                var carSalesTools = new CarSalesTool(
                    configuration, 
                    httpClientFactory,
                    aiInferenceService,
                    context,
                    GetCredentials());
                
                var getAvailableCarsForSaleTool = AIFunctionFactory.Create(
                    typeof(CarSalesTool).GetMethod(nameof(CarSalesTool.GetAvailableCarsForSale))!,
                    carSalesTools);
                
                var processCarInformationTool = AIFunctionFactory.Create(
                    typeof(CarSalesTool).GetMethod(nameof(CarSalesTool.ProcessCarInformation))!,
                    carSalesTools);
                
                tools.Add(getAvailableCarsForSaleTool);
                tools.Add(processCarInformationTool);
                
                break;
            
            case Feature.CarRegistration:
                var carRegistrationTools = new CarTools(context);

                var createCarTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.CreateCar))!,
                    carRegistrationTools);

                var updateCarTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.UpdateCar))!,
                    carRegistrationTools);

                var listCarsTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.ListCars))!,
                    carRegistrationTools);

                var getCarTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.GetCarById))!,
                    carRegistrationTools);

                var deleteCarTool = AIFunctionFactory.Create(
                    typeof(CarTools).GetMethod(nameof(CarTools.DeleteCar))!,
                    carRegistrationTools);

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