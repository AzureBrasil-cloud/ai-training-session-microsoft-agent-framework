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
        var client = CreateAgent(credentials, name, instructions);
        var thread = client.GetNewThread();

        return thread;
    }
}