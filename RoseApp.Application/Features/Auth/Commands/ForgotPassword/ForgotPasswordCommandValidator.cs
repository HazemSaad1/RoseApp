using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator
       : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }
    }
}
