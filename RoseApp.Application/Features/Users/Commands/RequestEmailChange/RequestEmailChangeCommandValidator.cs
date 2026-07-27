using FluentValidation;

namespace RoseApp.Application.Features.Users.Commands.RequestEmailChange
{
    public class RequestEmailChangeCommandValidator
      : AbstractValidator<RequestEmailChangeCommand>
    {
        public RequestEmailChangeCommandValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }
    }
}
