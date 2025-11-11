using System.Text.Json;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Agents.AI;
using ContosoAutoTech.Common;
using Microsoft.Agents.AI.Workflows;
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

    private static Workflow? _workflow;
    
    public async Task<(AgentRunResponse, AgentThread Thread)> CreateWorkflowRunAsync(
        Credentials credentials,
        string name,
        string instructions,
        string userMessage,
        string thread,
        IList<Microsoft.Agents.AI.AIAgent> aiAgents,
        IList<AITool>? tools = null,
        Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider>? aiContextProviderFactory = null)
    {
        var client = CreateAgentsClient(credentials);
        
        var orchestratorAgent = CreateAiAgent(
            client,
            instructions,
            name,
            tools,
            aiContextProviderFactory);

        var checkpointManager = CheckpointManager.Default;
        var workflowBuilder = new WorkflowBuilder(orchestratorAgent);

        foreach (var aiAgent in aiAgents)
        {
            workflowBuilder.AddEdge(orchestratorAgent, aiAgent);
        }

        if(_workflow is null)
        {
            var workflow = workflowBuilder.Build();
            _workflow = workflow;
        }
        var workflowAgent = _workflow.AsAgent("workflow-agent", "Workflow Agent", checkpointManager: checkpointManager);

        var resumedThread = workflowAgent.GetNewThread();
        
        if (string.CompareOrdinal(thread, "{}") != 0)
        {
            var reloaded = JsonSerializer.Deserialize<JsonElement>(thread, JsonSerializerOptions.Web);
            resumedThread = workflowAgent.DeserializeThread(reloaded, JsonSerializerOptions.Web);
        }
        
        var result = await workflowAgent.RunAsync(
            userMessage,
            resumedThread);

        return (result, resumedThread);
        
    }
    
    public Microsoft.Agents.AI.AIAgent CreateAIAgent(
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