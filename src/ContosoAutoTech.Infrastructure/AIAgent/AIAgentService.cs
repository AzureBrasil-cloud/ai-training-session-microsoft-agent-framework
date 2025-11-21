using Azure;
using Azure.AI.OpenAI;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    private const string ServiceName = "ContosoAutoTech.Api";
    
    private IChatClient CreateAgentsClient(Credentials credentials)
    {
        var client = new AzureOpenAIClient(
            new Uri(credentials.FoundryEndpoint),
            new AzureKeyCredential(credentials.FoundryApiKey))
            .GetChatClient(credentials.ModelDeploymentName)
            .AsIChatClient();

        return client;
    }
    
    private Microsoft.Agents.AI.AIAgent CreateAiAgent(
        IChatClient client,
        string instructions,
        string name,
        IList<AITool>? tools = null,
        Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider>? aiContextProviderFactory = null)
    {
        var chatClient = client
            .AsBuilder()
            .Use(getResponseFunc: ChatClientMiddleware, getStreamingResponseFunc: null)
            .Build();
        
        return chatClient.CreateAIAgent(
            new ChatClientAgentOptions
            {
                Name = name,
                Instructions = instructions,
                ChatOptions = tools is null ? null : new ChatOptions()
                {
                    Tools = tools,
                },
                AIContextProviderFactory = aiContextProviderFactory
            }
        ).AsBuilder()
        .Use(FunctionCallMiddleware)
        .UseOpenTelemetry(sourceName: ServiceName)
        .Build();;
        
    }
    
    async Task<ChatResponse> ChatClientMiddleware(IEnumerable<ChatMessage> message, ChatOptions? options, IChatClient innerChatClient, CancellationToken cancellationToken)
    {
        Console.WriteLine("Chat Client Middleware - Pre-Chat");
        var response = await innerChatClient.GetResponseAsync(message, options, cancellationToken);
        Console.WriteLine("Chat Client Middleware - Post-Chat");

        return response;
    }
    
    async ValueTask<object?> FunctionCallMiddleware(Microsoft.Agents.AI.AIAgent agent, FunctionInvocationContext context, Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Function Name: {context!.Function.Name} - Middleware 1 Pre-Invoke");
        var result = await next(context, cancellationToken);
        Console.WriteLine($"Function Name: {context!.Function.Name} - Middleware 1 Post-Invoke");

        return result;
    }
}