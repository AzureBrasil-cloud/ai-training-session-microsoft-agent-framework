using FluentValidation;

namespace ContosoAutoTech.Application.Agents.Models.Requests.Validators;

public class CreateRunRequestValidator : AbstractValidator<CreateRunRequest>
{
    public CreateRunRequestValidator()
    {
        RuleFor(x => x.ThreadId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.ThreadId)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.ThreadId)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.ThreadId)).Args);

        RuleFor(x => x.Message)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.Message)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.Message)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.Message)).Args);
        
        RuleFor(x => x.AgentName)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentName)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentName)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentName)).Args);
        
        RuleFor(x => x.AgentInstructions)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentInstructions)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentInstructions)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentInstructions)).Args);
        
        RuleFor(x => x.Feature)
            .Must(x => x > 0)
            .WithErrorCode(Errors.GreaterThanZero(nameof(CreateRunRequest.Feature)).Code.ToString())
            .WithMessage(Errors.GreaterThanZero(nameof(CreateRunRequest.Feature)).RawMessage)
            .WithState(x => Errors.GreaterThanZero(nameof(CreateRunRequest.Feature)).Args);
    }
}

public class CreateWorkflowRunRequestValidator : AbstractValidator<CreateWorkflowRunRequest>
{
    public CreateWorkflowRunRequestValidator()
    {
        RuleFor(x => x.ThreadId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.ThreadId)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.ThreadId)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.ThreadId)).Args);

        RuleFor(x => x.Message)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.Message)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.Message)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.Message)).Args);
        
        RuleFor(x => x.AgentName)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentName)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentName)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentName)).Args);
        
        RuleFor(x => x.AgentInstructions)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentInstructions)).Code.ToString())
            .WithMessage(Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentInstructions)).RawMessage)
            .WithState(x => Errors.NotBeNullOrEmpty(nameof(CreateRunRequest.AgentInstructions)).Args);
        
        RuleFor(x => x.Feature)
            .Must(x => x > 0)
            .WithErrorCode(Errors.GreaterThanZero(nameof(CreateRunRequest.Feature)).Code.ToString())
            .WithMessage(Errors.GreaterThanZero(nameof(CreateRunRequest.Feature)).RawMessage)
            .WithState(x => Errors.GreaterThanZero(nameof(CreateRunRequest.Feature)).Args);
    }
}