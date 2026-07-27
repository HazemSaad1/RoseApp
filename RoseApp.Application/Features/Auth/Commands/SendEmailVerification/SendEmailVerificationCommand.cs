using MediatR;
using RoseApp.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.Features.Auth.Commands.SendEmailVerification
{
    public record SendEmailVerificationCommand(string Email)
     : IRequest<ResponseViewModel<string>>;
}
