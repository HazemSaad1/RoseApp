using FluentValidation;

namespace RoseApp.Application.Features.Auth.Commands.ConfirmEmailVerification
{
    public class ConfirmEmailVerificationCommandValidator
       : AbstractValidator<ConfirmEmailVerificationCommand>
    {
        public ConfirmEmailVerificationCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .Length(6).WithMessage("Code must be 6 digits");
        }
    }
}
