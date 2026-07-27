using FluentValidation;

namespace RoseApp.Application.Features.Occasions.Commands.CreateOccasion;

public class CreateOccasionCommandValidator
    : AbstractValidator<CreateOccasionCommand>
{
    public CreateOccasionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100);
    }
}