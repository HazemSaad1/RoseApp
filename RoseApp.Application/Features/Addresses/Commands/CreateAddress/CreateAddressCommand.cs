using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Addresses.Commands.CreateAddress;

public record CreateAddressCommand(
    Guid UserId,
    string Label,
    string Street,
    string City,
    string State,
    string Country,
    string? ZipCode,
    bool IsDefault) : IRequest<ResponseViewModel<string>>;