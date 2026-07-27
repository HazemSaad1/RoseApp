using FluentValidation;

namespace RoseApp.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommandValidator
    : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required")
            .MaximumLength(50);
    }
}