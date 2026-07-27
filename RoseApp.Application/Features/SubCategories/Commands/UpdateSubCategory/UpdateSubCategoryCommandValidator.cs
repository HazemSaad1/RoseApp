using FluentValidation;

namespace RoseApp.Application.Features.SubCategories.Commands.UpdateSubCategory;

public class UpdateSubCategoryCommandValidator
    : AbstractValidator<UpdateSubCategoryCommand>
{
    public UpdateSubCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100);

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}