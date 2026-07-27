using FluentValidation;

namespace RoseApp.Application.Features.Users.Commands.ConfirmEmailChange
{
    public class ConfirmEmailChangeCommandValidator
       : AbstractValidator<ConfirmEmailChangeCommand>
    {
        public ConfirmEmailChangeCommandValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .Length(6).WithMessage("Code must be 6 digits");
        }
    }
}
