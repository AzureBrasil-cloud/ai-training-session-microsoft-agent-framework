using FluentValidation;

namespace ContosoAutoTech.Application.Agents.Models.Requests.Validators;

public class CreateThreadRequestValidator : AbstractValidator<CreateThreadRequest>
{
    public CreateThreadRequestValidator()
    {
        RuleFor(x => x.Feature)
            .Must(x => x > 0)
            .WithErrorCode(Errors.GreaterThanZero(nameof(CreateThreadRequest.Feature)).Code.ToString())
            .WithMessage(Errors.GreaterThanZero(nameof(CreateThreadRequest.Feature)).RawMessage)
            .WithState(x => Errors.GreaterThanZero(nameof(CreateThreadRequest.Feature)).Args);
    }
}