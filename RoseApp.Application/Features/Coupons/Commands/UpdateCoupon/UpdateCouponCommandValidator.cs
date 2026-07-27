using FluentValidation;

namespace RoseApp.Application.Features.Coupons.Commands.UpdateCoupon;

public class UpdateCouponCommandValidator
    : AbstractValidator<UpdateCouponCommand>
{
    public UpdateCouponCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(50)
            .Matches("^[A-Z0-9_-]+$")
            .WithMessage("Code must contain only uppercase letters, numbers, hyphens and underscores");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0).WithMessage("Discount value must be greater than 0");

        RuleFor(x => x.DiscountValue)
            .LessThanOrEqualTo(100)
            .When(x => x.DiscountType == Domain.Enums.DiscountType.Percentage)
            .WithMessage("Percentage discount cannot exceed 100%");

        RuleFor(x => x.MinOrderAmount)
            .GreaterThan(0)
            .When(x => x.MinOrderAmount.HasValue);

        RuleFor(x => x.MaxUses)
            .GreaterThan(0)
            .When(x => x.MaxUses.HasValue);

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiresAt.HasValue)
            .WithMessage("Expiry date must be in the future");
    }
}