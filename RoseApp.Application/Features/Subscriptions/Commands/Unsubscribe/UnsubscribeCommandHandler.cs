using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Subscriptions.Commands.Unsubscribe;

public class UnsubscribeCommandHandler
    : IRequestHandler<UnsubscribeCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UnsubscribeCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UnsubscribeCommand request,
        CancellationToken cancellationToken)
    {
        var subscription = await _context.Set<NewsletterSubscription>()
            .FirstOrDefaultAsync(s => s.Email == request.Email, cancellationToken);

        if (subscription is null)
            return ResponseViewModel<string>.Fail("Email not found", 404);

        _context.Set<NewsletterSubscription>().Remove(subscription);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Unsubscribed successfully");
    }
}