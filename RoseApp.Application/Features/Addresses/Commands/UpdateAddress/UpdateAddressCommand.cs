using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Addresses.Commands.UpdateAddress;

public record UpdateAddressCommand(
    Guid UserId,
    Guid AddressId,
    string Label,
    string Street,
    string City,
    string State,
    string Country,
    string? ZipCode,
    bool IsDefault) : IRequest<ResponseViewModel<string>>;