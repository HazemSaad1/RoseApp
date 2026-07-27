using FluentValidation;

namespace RoseApp.Application.Features.Notifications.Commands.CreateNotification;

public class CreateNotificationCommandValidator
    : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200);

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(1000);
    }
}