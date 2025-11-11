using System.Diagnostics;
using System.Text.Json;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Application.Agents.Models.Requests.Validators;
using ContosoAutoTech.Application.Agents.Models.Results;
using ContosoAutoTech.Common;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using Thread = ContosoAutoTech.Data.Entities.Thread;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    public async Task<Result<MessageResult>> RunWorkflowAsync(CreateWorkflowRunRequest request)
    {
        using Activity? activity = ActivitySource.StartActivity();
        
        var validationResult = await new CreateWorkflowRunRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var error = ErrorHandler.CreateErrorFromValidationResult(validationResult);
            logger.LogInformation("{Method} - {Message}", nameof(RunAsync), error.Message);
            
            return Result<MessageResult>.Failure(error);
        }

        logger.LogInformation("Starting run ...");
        
        var thread = context.Threads.FirstOrDefault(x => x.Id == Guid.Parse(request.ThreadId));

        if (thread is null)
        {
            var error = Errors.EntityNotFound(nameof(Thread), request.ThreadId);
            logger.LogInformation("{Method} - {Message}", nameof(RunAsync), error.Message);
            
            return Result<MessageResult>.Failure(error);
        }

        var credentials = GetCredentials();

        var agents = new List<AIAgent>();
        var mcps = new List<McpClient>();
        
        foreach (var agentRequest in request.Agents)
        {
            var (mcpTools, mcpClients) = await GetToolsWithMcpClient(agentRequest.Feature);

            var tools = GetToolsByFeature(request.Feature).ToList();
            tools.AddRange(mcpTools);
            var agent = aiAgentService.CreateAIAgent(credentials, agentRequest.AgentInstructions,
                agentRequest.AgentName, tools);
            agents.Add(agent);
            mcps.AddRange(mcpClients);
        }
        
        try
        {
            // Create and execute the run
            var (runResult, updatedThread) = await aiAgentService.CreateWorkflowRunAsync(
                credentials,
                request.AgentName.Trim(),
                request.AgentInstructions.Trim(),
                request.Message.Trim(),
                thread.State,
                agents);
    
            // Save the new thread updated
            var serializedJson = updatedThread.Serialize(JsonSerializerOptions.Web).GetRawText();
            thread.State = serializedJson;
        
            // Save changes to the database
            await context.SaveChangesAsync();
    
            return Result<MessageResult>.Success(new MessageResult(
                Role.Agent, 
                runResult.Text,
                new TokenUsage(
                    runResult.Usage?.InputTokenCount, 
                    runResult.Usage?.OutputTokenCount, 
                    runResult.Usage?.TotalTokenCount)));
        }
        finally
        {
            foreach (var mcpClient in mcps)
            {
                await mcpClient.DisposeAsync();
            }
        }
    }
}