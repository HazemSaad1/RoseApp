using FluentValidation;

namespace RoseApp.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator
    : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .When(x => x.Comment is not null);
    }
}