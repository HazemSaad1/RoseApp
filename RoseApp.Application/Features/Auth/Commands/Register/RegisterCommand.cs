using MediatR;
using RoseApp.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(
      string FirstName,
      string LastName,
      string UserName,
      string Email,
      string Password) : IRequest<ResponseViewModel<string>>;
}
