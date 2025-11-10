using System.Diagnostics;
using System.Text.Json;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Application.Agents.Models.Requests.Validators;
using ContosoAutoTech.Application.Agents.Models.Results;
using Microsoft.Extensions.Logging;
using Thread = System.Threading.Thread;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    private static readonly ActivitySource ActivitySource =  InstrumentationConfig.ActivitySource;
    public async Task<Result<MessageResult>> RunAsync(CreateRunRequest request)
    {
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

        using Activity activity = ActivitySource.StartActivity("Run");
        
        var credentials = GetCredentials();
        
        // Create and execute the run
        var (runResult, updatedThread, firstTruncatedMessage) = await aiAgentService.CreateRunAsync(
            credentials,
            request.AgentName.Trim(),
            request.AgentInstructions.Trim(),
            request.Message.Trim(),
            thread.State,
            GetToolsByFeature(request.Feature));
        
        // Save the new thread updated
        var serializedJson = updatedThread.Serialize(JsonSerializerOptions.Web).GetRawText();
        thread.State = serializedJson;

        // If this is the first message that caused the thread to be created, save it
        if (!string.IsNullOrWhiteSpace(firstTruncatedMessage))
        {
            thread.FirstTruncatedMessage = firstTruncatedMessage;
        }

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