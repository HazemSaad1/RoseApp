using FluentValidation;

namespace RoseApp.Application.Features.Blogs.Commands.CreateBlog;

public class CreateBlogCommandValidator
    : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required");
    }
}