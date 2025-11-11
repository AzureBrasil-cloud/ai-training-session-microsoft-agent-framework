using System.Diagnostics;
using ContosoAutoTech.Application.Agents.Models.Results;
using ContosoAutoTech.Application.AiInference.Models.Requests;
using ContosoAutoTech.Application.AiInference.Models.Requests.Validators;
using ContosoAutoTech.Application.AiInference.Models.Results;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Extensions.Logging;

namespace ContosoAutoTech.Application.AiInference;

public partial class AiInferenceApplicationService
{
    private static readonly ActivitySource ActivitySource = InstrumentationConfig.ActivitySource;

    public async Task<Result<ChatCompletionResult>> CompleteAsync(ChatCompletionRequest request)
    {
        using Activity? activity = ActivitySource.StartActivity();
        
        var validationResult = await new ChatCompletionRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var error = ErrorHandler.CreateErrorFromValidationResult(validationResult);
            logger.LogWarning("{Method} - Validation failed: {Message}", nameof(CompleteAsync), error.Message);
            activity?.SetTag("validation.failed", true);
            
            return Result<ChatCompletionResult>.Failure(error);
        }

        var credentials = GetCredentials();
        
        var useFinedClassificationModel =
            Convert.ToBoolean(configuration["Application:UserClassificationFinedTunedModel"]);

        if (useFinedClassificationModel)
        {
            credentials = credentials with
            {
                ModelDeploymentName = configuration["AiFoundry:FinedTunedUserClassificationModel"]!
            };
        }

        try
        {
            logger.LogInformation("{Method} - Starting chat completion", nameof(CompleteAsync));
            var response = await aiInferenceService.CompleteAsync(
                credentials,
                request.Instructions!.Trim(),
                request.Message!.Trim());

            var result = new ChatCompletionResult(
                Content: response.Text ?? string.Empty,
                Usage: response.Usage != null
                    ? new TokenUsage(
                        response.Usage.InputTokenCount,
                        response.Usage.OutputTokenCount,
                        response.Usage.TotalTokenCount)
                    : null);

            logger.LogInformation("{Method} - Chat completion successful. Tokens used: {TotalTokens}", 
                nameof(CompleteAsync), 
                result.Usage!.Total ?? 0);

            return Result<ChatCompletionResult>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Method} - Unexpected error during chat completion", nameof(CompleteAsync));
            return Result<ChatCompletionResult>.Failure(Errors.AiInferenceError());
        }
    }
}

