using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Addresses.Commands.DeleteAddress;

public record DeleteAddressCommand(Guid UserId, Guid AddressId)
    : IRequest<ResponseViewModel<string>>;