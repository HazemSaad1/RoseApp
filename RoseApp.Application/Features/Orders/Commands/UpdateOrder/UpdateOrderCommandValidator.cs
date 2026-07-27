using FluentValidation;

namespace RoseApp.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandValidator
    : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status");
    }
}