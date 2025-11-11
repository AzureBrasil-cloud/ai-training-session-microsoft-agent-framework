using System.Diagnostics;
using System.Text.Json;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Application.Agents.Models.Requests.Validators;
using ContosoAutoTech.Application.Agents.Models.Results;
using ContosoAutoTech.Common;
using ContosoAutoTech.Data.Entities;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;
using Thread = System.Threading.Thread;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    private static readonly ActivitySource ActivitySource =  InstrumentationConfig.ActivitySource;
    public async Task<Result<MessageResult>> RunAsync(CreateRunRequest request)
    {
        using Activity? activity = ActivitySource.StartActivity();
        
        var validationResult = await new CreateRunRequestValidator().ValidateAsync(request);

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

        // Save the first message sent by the user for context
        // if the thread is new
        if (string.CompareOrdinal(thread.State, "{}") == 0)
        {
            thread.FirstTruncatedMessage = request.Message.Truncate();
        }

        // Create and execute the run
        var (runResult, updatedThread) = await aiAgentService.CreateRunAsync(
            credentials,
            request.AgentName.Trim(),
            request.AgentInstructions.Trim(),
            request.Message.Trim(),
            thread.State,
            GetToolsByFeature(request.Feature),
            GetAiContextProviderByFeature(request.Feature));
        
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
}