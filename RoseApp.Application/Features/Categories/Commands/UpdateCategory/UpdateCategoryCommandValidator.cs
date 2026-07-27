using FluentValidation;

namespace RoseApp.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator
    : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100);
    }
}