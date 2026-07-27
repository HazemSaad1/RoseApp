using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Addresses;

namespace RoseApp.Application.Features.Addresses.Queries.GetAddresses;

public record GetAddressesQuery(Guid UserId)
    : IRequest<ResponseViewModel<List<AddressDto>>>;