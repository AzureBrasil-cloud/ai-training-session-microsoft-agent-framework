using FluentValidation;

namespace ContosoAutoTech.Application.AiInference.Models.Requests.Validators;

public class ChatCompletionRequestValidator : AbstractValidator<ChatCompletionRequest>
{
    public ChatCompletionRequestValidator()
    {
        RuleFor(x => x.Instructions)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(ChatCompletionRequest.Instructions)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(ChatCompletionRequest.Instructions)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(ChatCompletionRequest.Instructions)).Args);

        RuleFor(x => x.Message)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(ChatCompletionRequest.Message)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(ChatCompletionRequest.Message)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(ChatCompletionRequest.Message)).Args);
    }
}

