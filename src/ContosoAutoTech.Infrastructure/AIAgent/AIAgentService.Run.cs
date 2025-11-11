using System.Text.Json;
using Azure.AI.OpenAI;
using Azure.Identity;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using OpenAI;
using ContosoAutoTech.Common;
using Microsoft.Extensions.AI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public async Task<(AgentRunResponse, AgentThread Thread, string? FirstTruncatedMessage)> CreateRunAsync(
        Credentials credentials,
        string name,
        string instructions,
        string userMessage,
        string thread,
        IList<AITool>? tools = null)
    {
        var client = CreateAgentsClient(credentials);

        var agent = CreateAiAgent(
            client,
            instructions,
            name,
            tools);
        
        var reloaded = JsonSerializer.Deserialize<JsonElement>(thread, JsonSerializerOptions.Web);
        var resumedThread = agent.DeserializeThread(reloaded, JsonSerializerOptions.Web);
        
        var result = await agent.RunAsync(
            userMessage, 
            resumedThread);

        return string.CompareOrdinal(thread, "{}") == 0 ? 
            (result, resumedThread, userMessage.Truncate()) : 
            (result, resumedThread, null);
    }
}