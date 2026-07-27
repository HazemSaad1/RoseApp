using FluentValidation;

namespace RoseApp.Application.Features.Blogs.Commands.UpdateBlog;

public class UpdateBlogCommandValidator
    : AbstractValidator<UpdateBlogCommand>
{
    public UpdateBlogCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required");
    }
}