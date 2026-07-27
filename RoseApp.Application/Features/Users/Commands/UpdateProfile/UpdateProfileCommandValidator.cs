using FluentValidation;

namespace RoseApp.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidator
     : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20)
                .When(x => x.PhoneNumber is not null);
        }
    }
}
