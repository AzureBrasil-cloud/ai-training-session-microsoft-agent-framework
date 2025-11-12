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

    private static string? _workflowId;
    private static Workflow? _workflow;
    private static CheckpointManager _checkpointManager = CheckpointManager.CreateInMemory();
    
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
        
        var workflowBuilder = new WorkflowBuilder(orchestratorAgent);
        
        foreach (var aiAgent in aiAgents)
        {
            workflowBuilder.AddEdge(orchestratorAgent, aiAgent);
        }
        var workflowSignature = $"handoff|{string.Join("|", aiAgents.Select(a => a.Name))}".GetHashCode().ToString();
        
        if (_workflow == null || _workflowId != workflowSignature)
        {
            _workflow = workflowBuilder.Build();
            _workflowId = workflowSignature;
        }

        var checkpointManager = _checkpointManager;

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