using Azure;
using Azure.AI.OpenAI;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Extensions.AI;

namespace ContosoAutoTech.Infrastructure.AiInference;

public sealed partial class AiInferenceService
{
    private const string ServiceName = "ContosoAutoTech.Api";
    
    private IChatClient CreateChatClient(
        Credentials credentials)
    {
        return new AzureOpenAIClient(  
                new Uri(credentials.FoundryEndpoint),   
                new AzureKeyCredential(credentials.FoundryApiKey))
            .GetChatClient(credentials.ModelDeploymentName)
            .AsIChatClient() 
            .AsBuilder()
            .UseOpenTelemetry(sourceName: ServiceName, configure: (cfg) => cfg.EnableSensitiveData = true)   
            .Build();
    }
}