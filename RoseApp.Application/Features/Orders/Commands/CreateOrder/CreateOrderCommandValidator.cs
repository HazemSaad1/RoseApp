using FluentValidation;

namespace RoseApp.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.AddressId)
            .NotEmpty().WithMessage("Address is required");
    }
}