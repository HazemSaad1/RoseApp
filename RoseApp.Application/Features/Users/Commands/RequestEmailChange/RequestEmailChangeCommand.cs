using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Users.Commands.RequestEmailChange
{
    public record RequestEmailChangeCommand(
      Guid UserId,
      string NewEmail) : IRequest<ResponseViewModel<string>>;
}
