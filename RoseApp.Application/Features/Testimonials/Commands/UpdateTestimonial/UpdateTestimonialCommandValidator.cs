using FluentValidation;

namespace RoseApp.Application.Features.Testimonials.Commands.UpdateTestimonial;

public class UpdateTestimonialCommandValidator
    : AbstractValidator<UpdateTestimonialCommand>
{
    public UpdateTestimonialCommandValidator()
    {
        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("Author name is required")
            .MaximumLength(100);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(1000);

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");
    }
}