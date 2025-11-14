using System.Text.Json;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using ContosoAutoTech.Common;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public async Task<(AgentRunResponse, AgentThread Thread)> CreateRunMultiAgentsAsync(
        Credentials credentials,
        string name,
        string instructions,
        string userMessage,
        string thread,
        IList<Microsoft.Agents.AI.AIAgent> aiAgents,
        Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider>? aiContextProviderFactory = null)
    {
        var client = CreateAgentsClient(credentials);
        
        var orchestratorAgent = CreateAiAgent(
            client,
            instructions,
            name,
            aiAgents.Select(AITool (x) => x.AsAIFunction()).ToList(),
            aiContextProviderFactory);
        
        var reloaded = JsonSerializer.Deserialize<JsonElement>(thread, JsonSerializerOptions.Web);
        var resumedThread = orchestratorAgent.DeserializeThread(reloaded, JsonSerializerOptions.Web);

        var result = await orchestratorAgent.RunAsync(
            userMessage,
            resumedThread);

        return (result, resumedThread);
        
    }
}