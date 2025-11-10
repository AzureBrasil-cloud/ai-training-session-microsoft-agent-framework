using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using OpenAI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public virtual ChatClientAgent CreateAgentAsync(
        Credentials credentials, 
        string name,
        string instructions)
    {
        var client = CreateAgentsClient(credentials);

        ChatClientAgent agent = client.CreateAIAgent(name: name, instructions: instructions);
        
        return agent;
    }
}