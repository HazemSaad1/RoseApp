using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.Features.Auth.Commands.SendEmailVerification
{
    public class SendEmailVerificationCommandValidator
     : AbstractValidator<SendEmailVerificationCommand>
    {
        public SendEmailVerificationCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }
    }
}
