using FluentValidation;

namespace RoseApp.Application.Features.SubCategories.Commands.CreateSubCategory;

public class CreateSubCategoryCommandValidator
    : AbstractValidator<CreateSubCategoryCommand>
{
    public CreateSubCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100);

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}