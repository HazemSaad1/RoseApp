using FluentValidation;

namespace RoseApp.Application.Features.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandValidator
    : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required")
            .MaximumLength(200);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100);

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required")
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100);

        RuleFor(x => x.ZipCode)
            .MaximumLength(20)
            .When(x => x.ZipCode is not null);

        RuleFor(x => x.Label)
            .MaximumLength(50)
            .When(x => x.Label is not null);
    }
}