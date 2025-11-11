using System.Text.Json;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Application.Agents.Models.Requests.Validators;
using ContosoAutoTech.Application.Agents.Models.Results;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    public async Task<Result<ThreadResult>> CreateThreadAsync(CreateThreadRequest request)
    {
        var validationResult = await new CreateThreadRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var error = ErrorHandler.CreateErrorFromValidationResult(validationResult);
            logger.LogInformation("{Method} - {Message}", nameof(CreateThreadAsync), error.Message);
            
            return Result<ThreadResult>.Failure(error);
        }
        
        var credentials = GetCredentials();
       
        var aiThread = aiAgentService.CreateThread(
            credentials,
            string.Empty,
            string.Empty);
        
        // State to string
        // Serialize the thread state
        var serializedJson = aiThread.Serialize(JsonSerializerOptions.Web).GetRawText();

        var thread = new ContosoAutoTech.Data.Entities.Thread(serializedJson, request.Feature);
        
        await context.Threads.AddAsync(thread);
        await context.SaveChangesAsync();
        
        return new ThreadResult(thread.Id);
    }
}