using FluentValidation;

namespace RoseApp.Application.Features.Cart.Commands.UpdateCartItem;

public class UpdateCartItemCommandValidator
    : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}