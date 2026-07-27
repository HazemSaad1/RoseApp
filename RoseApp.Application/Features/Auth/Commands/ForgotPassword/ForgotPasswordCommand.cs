using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Auth.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email)
      : IRequest<ResponseViewModel<string>>;
}
