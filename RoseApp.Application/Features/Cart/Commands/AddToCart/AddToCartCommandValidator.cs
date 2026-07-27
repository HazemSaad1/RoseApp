using FluentValidation;

namespace RoseApp.Application.Features.Cart.Commands.AddToCart;

public class AddToCartCommandValidator
    : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}