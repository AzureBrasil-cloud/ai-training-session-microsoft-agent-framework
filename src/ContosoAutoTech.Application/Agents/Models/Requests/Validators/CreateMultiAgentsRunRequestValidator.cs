using FluentValidation;

namespace ContosoAutoTech.Application.Agents.Models.Requests.Validators;

public class CreateMultiAgentsRunRequestValidator : AbstractValidator<CreateMultiAgentsRunRequest>
{
    public CreateMultiAgentsRunRequestValidator()
    {
        RuleFor(x => x.ThreadId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.ThreadId)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.ThreadId)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.ThreadId)).Args);

        RuleFor(x => x.Message)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.Message)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.Message)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.Message)).Args);
        
        RuleFor(x => x.AgentName)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.AgentName)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.AgentName)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.AgentName)).Args);
        
        RuleFor(x => x.AgentInstructions)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.AgentInstructions)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.AgentInstructions)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateMultiAgentsRunRequest.AgentInstructions)).Args);
        
        RuleFor(x => x.Feature)
            .Must(x => x > 0)
            .WithErrorCode(Errors.GreaterThanZero(nameof(CreateMultiAgentsRunRequest.Feature)).Code.ToString())
            .WithMessage(Errors.GreaterThanZero(nameof(CreateMultiAgentsRunRequest.Feature)).RawMessage)
            .WithState(x => Errors.GreaterThanZero(nameof(CreateMultiAgentsRunRequest.Feature)).Args);
    }
}