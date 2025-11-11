using Azure;
using Azure.AI.OpenAI;
using ContosoAutoTech.Infrastructure.Email;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    private const string ServiceName = "ContosoAutoTech.Api";
    
    private ChatClient CreateAgentsClient(Credentials credentials)
    {
        return new AzureOpenAIClient(  
                new Uri(credentials.FoundryEndpoint),   
                new AzureKeyCredential(credentials.FoundryApiKey))  
            .GetChatClient(credentials.ModelDeploymentName);      
    }
    
    private Microsoft.Agents.AI.AIAgent CreateAiAgent(
        ChatClient client,
        string instructions,
        string name,
        IList<AITool>? tools = null)
    {
        return  client.CreateAIAgent(
                instructions: instructions,
                name: name,
                tools: tools)
            .AsBuilder()
            .UseOpenTelemetry(sourceName: ServiceName)
            .Build();
    }
}