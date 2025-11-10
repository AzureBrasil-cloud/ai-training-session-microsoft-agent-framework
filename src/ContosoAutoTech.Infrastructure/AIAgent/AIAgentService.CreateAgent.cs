using ContosoAutoTech.Infrastructure.AIAgent.Models;
using ContosoAutoTech.Infrastructure.Azure.Shared;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public virtual async Task<Agent> CreateAgentAsync(
        Credentials credentials, 
        string name,
        string instructions,
        string aiModel)
    {
        var client = CreateAgentsClient(credentials);
        
        var agentResponse = await client.CreateAgentAsync(
            model: aiModel,
            name: name,
            instructions: instructions);

        return new Agent(
            agentResponse.Value.Id,
            agentResponse.Value.Name,
            agentResponse.Value.Instructions);
    }
}