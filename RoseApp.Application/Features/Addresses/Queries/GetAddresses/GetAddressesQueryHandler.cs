using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Addresses;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Addresses.Queries.GetAddresses;

public class GetAddressesQueryHandler
    : IRequestHandler<GetAddressesQuery, ResponseViewModel<List<AddressDto>>>
{
    private readonly IDbContext _context;

    public GetAddressesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<List<AddressDto>>> Handle(
        GetAddressesQuery request,
        CancellationToken cancellationToken)
    {
        var addresses = await _context.Set<Address>()
            .AsNoTracking()
            .Where(a => a.UserId == request.UserId)
            .Select(a => new AddressDto
            {
                Id = a.Id,
                Label = a.Label,
                Street = a.Street,
                City = a.City,
                State = a.State,
                Country = a.Country,
                ZipCode = a.ZipCode,
                IsDefault = a.IsDefault
            })
            .ToListAsync(cancellationToken);

        return ResponseViewModel<List<AddressDto>>.Success(addresses);
    }
}