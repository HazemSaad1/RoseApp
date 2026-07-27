using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Users.Commands.ConfirmEmailChange
{
    public record ConfirmEmailChangeCommand(
      Guid UserId,
      string NewEmail,
      string Code) : IRequest<ResponseViewModel<string>>;
}
