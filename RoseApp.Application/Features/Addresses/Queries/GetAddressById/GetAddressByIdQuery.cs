using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Addresses;

namespace RoseApp.Application.Features.Addresses.Queries.GetAddressById;

public record GetAddressByIdQuery(Guid UserId, Guid AddressId)
    : IRequest<ResponseViewModel<AddressDto>>;