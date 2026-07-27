using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator
       : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password must be different from current password");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
        }
    }
}
