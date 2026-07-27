using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Addresses;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Addresses.Queries.GetAddressById;

public class GetAddressByIdQueryHandler
    : IRequestHandler<GetAddressByIdQuery, ResponseViewModel<AddressDto>>
{
    private readonly IDbContext _context;

    public GetAddressByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<AddressDto>> Handle(
        GetAddressByIdQuery request,
        CancellationToken cancellationToken)
    {
        var address = await _context.Set<Address>()
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.AddressId
                && a.UserId == request.UserId, cancellationToken);

        if (address is null)
            throw new NotFoundException(nameof(Address), request.AddressId);

        return ResponseViewModel<AddressDto>.Success(new AddressDto
        {
            Id = address.Id,
            Label = address.Label,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country,
            ZipCode = address.ZipCode,
            IsDefault = address.IsDefault
        });
    }
}