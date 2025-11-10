using System.Text.Json;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using OpenAI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public async Task<(AgentRunResponse, AgentThread Thread)> CreateRunAsync(
        Credentials credentials,
        string name,
        string instructions,
        string userMessage,
        string thread)
    {
        var client = CreateAgentsClient(credentials);

        var agent = client.CreateAIAgent(
            instructions: instructions,
            name: name);
        
        var reloaded = JsonSerializer.Deserialize<JsonElement>(thread, JsonSerializerOptions.Web);
        var resumedThread = agent.DeserializeThread(reloaded, JsonSerializerOptions.Web);
        
        var result = await agent.RunAsync(userMessage, resumedThread);

        return (result, resumedThread);
    }
}