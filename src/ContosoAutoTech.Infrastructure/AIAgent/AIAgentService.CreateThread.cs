using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public virtual AgentThread CreateThread(
        Credentials credentials,
        string name,
        string instructions)
    {
        var client = CreateAgentsClient(credentials);

        var agent = CreateAiAgent(
            client,
            instructions,
            name,
            []);
        
        var thread = agent.GetNewThread();

        return thread;
    }
}