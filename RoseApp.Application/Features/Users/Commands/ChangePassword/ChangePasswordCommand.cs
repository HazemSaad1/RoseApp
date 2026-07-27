using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(
      Guid UserId,
      string CurrentPassword,
      string NewPassword,
      string ConfirmPassword) : IRequest<ResponseViewModel<string>>;
}
