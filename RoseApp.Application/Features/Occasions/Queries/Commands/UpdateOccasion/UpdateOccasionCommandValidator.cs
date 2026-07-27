using FluentValidation;

namespace RoseApp.Application.Features.Occasions.Commands.UpdateOccasion;

public class UpdateOccasionCommandValidator
    : AbstractValidator<UpdateOccasionCommand>
{
    public UpdateOccasionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100);
    }
}