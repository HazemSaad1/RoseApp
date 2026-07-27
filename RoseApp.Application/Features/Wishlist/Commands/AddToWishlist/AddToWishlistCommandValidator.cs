using FluentValidation;

namespace RoseApp.Application.Features.Wishlist.Commands.AddToWishlist;

public class AddToWishlistCommandValidator
    : AbstractValidator<AddToWishlistCommand>
{
    public AddToWishlistCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required");
    }
}