using FluentValidation;

namespace RoseApp.Application.Features.Subscriptions.Commands.Unsubscribe;

public class UnsubscribeCommandValidator
    : AbstractValidator<UnsubscribeCommand>
{
    public UnsubscribeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}