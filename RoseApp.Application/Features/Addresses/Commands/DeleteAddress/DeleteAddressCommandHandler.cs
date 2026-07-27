// Features/Addresses/Commands/DeleteAddress/DeleteAddressCommandHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Addresses.Commands.DeleteAddress;

public class DeleteAddressCommandHandler
    : IRequestHandler<DeleteAddressCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteAddressCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteAddressCommand request,
        CancellationToken cancellationToken)
    {
        var address = await _context.Set<Address>()
            .FirstOrDefaultAsync(a => a.Id == request.AddressId
                && a.UserId == request.UserId, cancellationToken);

        if (address is null)
            throw new NotFoundException(nameof(Address), request.AddressId);

        _context.Set<Address>().Remove(address);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Address deleted successfully");
    }
}