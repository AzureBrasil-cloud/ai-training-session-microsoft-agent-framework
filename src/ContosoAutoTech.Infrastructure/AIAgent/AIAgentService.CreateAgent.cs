using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using OpenAI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public virtual ChatClientAgent CreateAgent(
        Credentials credentials,
        string name,
        string instructions)
    {
        var client = CreateAgentsClient(credentials);
        var agent = client.CreateAIAgent(name: name, instructions: instructions);
        return agent;
    }
}