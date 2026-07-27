using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Auth.Commands.ResetPassword
{

    public record ResetPasswordCommand(
        string Email,
        string Code,
        string NewPassword,
        string ConfirmPassword) : IRequest<ResponseViewModel<string>>;
}
