using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Auth.Commands.ConfirmEmailVerification
{
    public record ConfirmEmailVerificationCommand(string Email, string Code)
     : IRequest<ResponseViewModel<string>>;
}
