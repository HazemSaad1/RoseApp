using FluentValidation;

namespace RoseApp.Application.Features.Subscriptions.Commands.Subscribe;

public class SubscribeCommandValidator
    : AbstractValidator<SubscribeCommand>
{
    public SubscribeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}