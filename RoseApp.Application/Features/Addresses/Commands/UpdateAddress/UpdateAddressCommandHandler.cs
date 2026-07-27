using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler
    : IRequestHandler<UpdateAddressCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateAddressCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateAddressCommand request,
        CancellationToken cancellationToken)
    {
        var address = await _context.Set<Address>()
            .FirstOrDefaultAsync(a => a.Id == request.AddressId
                && a.UserId == request.UserId, cancellationToken);

        if (address is null)
            throw new NotFoundException(nameof(Address), request.AddressId);

        if (request.IsDefault && !address.IsDefault)
        {
            var existingDefaults = await _context.Set<Address>()
                .Where(a => a.UserId == request.UserId && a.IsDefault)
                .ToListAsync(cancellationToken);

            foreach (var existing in existingDefaults)
            {
                existing.IsDefault = false;
                _context.Entry(existing).State = EntityState.Modified;
            }
        }

        address.Label = request.Label;
        address.Street = request.Street;
        address.City = request.City;
        address.State = request.State;
        address.Country = request.Country;
        address.ZipCode = request.ZipCode;
        address.IsDefault = request.IsDefault;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Address updated successfully");
    }
}