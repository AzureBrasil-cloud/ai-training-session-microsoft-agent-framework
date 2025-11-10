using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using OpenAI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public virtual async Task<ChatClientAgent> CreateAgent(
        Credentials credentials,
        string name,
        string instructions)
    {
        var client = CreateAgentsClient(credentials);

        var agent = client.CreateAIAgent(name: name, instructions: instructions);
        
        var result = await agent.RunAsync("Tell me a joke about a pirate.");
        
        return agent;
    }
}