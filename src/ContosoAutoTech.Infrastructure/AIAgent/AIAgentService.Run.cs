using System.Text.Json;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using ContosoAutoTech.Common;
using Microsoft.Extensions.AI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public async Task<(AgentRunResponse, AgentThread Thread)> CreateRunAsync(
        Credentials credentials,
        string name,
        string instructions,
        string userMessage,
        string thread,
        IList<AITool>? tools = null,
        Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider>? aiContextProviderFactory = null)
    {
        var client = CreateAgentsClient(credentials);

        var agent = CreateAiAgent(
            client,
            instructions,
            name,
            tools,
            aiContextProviderFactory);

        var reloaded = JsonSerializer.Deserialize<JsonElement>(thread, JsonSerializerOptions.Web);
        var resumedThread = agent.DeserializeThread(reloaded, JsonSerializerOptions.Web);

        var result = await agent.RunAsync(
            userMessage,
            resumedThread);

        return (result, resumedThread);
    }
}