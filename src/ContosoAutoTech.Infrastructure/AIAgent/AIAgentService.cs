using Azure;
using Azure.AI.OpenAI;
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
        IList<AITool>? tools = null,
        Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider>? aiContextProviderFactory = null)
    {
        return  client
            .CreateAIAgent(new ChatClientAgentOptions
            {
                Name = name,
                Instructions = instructions,
                ChatOptions = tools is null ? null : new ChatOptions()
                {
                    Tools = tools,
                },
                AIContextProviderFactory = aiContextProviderFactory
            })
            .AsBuilder()
            .UseOpenTelemetry(sourceName: ServiceName)
            .Build();
    }
}