using FluentValidation;

namespace RoseApp.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}