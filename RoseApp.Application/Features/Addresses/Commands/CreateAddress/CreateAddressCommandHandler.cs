using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler
    : IRequestHandler<CreateAddressCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateAddressCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateAddressCommand request,
        CancellationToken cancellationToken)
    {
        if (request.IsDefault)
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

        var address = new Address
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Label = request.Label,
            Street = request.Street,
            City = request.City,
            State = request.State,
            Country = request.Country,
            ZipCode = request.ZipCode,
            IsDefault = request.IsDefault
        };

        await _context.Set<Address>().AddAsync(address, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Address created successfully", "Created", 201);
    }
}