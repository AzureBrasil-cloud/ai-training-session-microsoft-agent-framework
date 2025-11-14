using System.Text.Json;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using ContosoAutoTech.Common;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public Microsoft.Agents.AI.AIAgent CreateAiAgent(
        Credentials credentials,
        string instructions,
        string name,
        IList<AITool>? tools = null,
        Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider>? aiContextProviderFactory =
            null)
    {
        var client = CreateAgentsClient(credentials);
        var agent = CreateAiAgent(
            client,
            instructions,
            name,
            tools,
            aiContextProviderFactory);
        return agent;
    }
}