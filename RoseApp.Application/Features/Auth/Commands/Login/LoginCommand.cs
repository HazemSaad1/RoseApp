using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Auth;

namespace RoseApp.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password)
     : IRequest<ResponseViewModel<AuthResponseDto>>;
}
