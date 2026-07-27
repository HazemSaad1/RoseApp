using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Users.Commands.DeleteAccount
{
    public record DeleteAccountCommand(Guid UserId)
      : IRequest<ResponseViewModel<string>>;
}
