using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Subscriptions.Commands.Subscribe;

public class SubscribeCommandHandler
    : IRequestHandler<SubscribeCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public SubscribeCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        SubscribeCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Set<NewsletterSubscription>()
            .AnyAsync(s => s.Email == request.Email, cancellationToken);

        if (exists)
            return ResponseViewModel<string>.Fail(
                "Email already subscribed", 409);

        var subscription = new NewsletterSubscription
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            SubscribedAt = DateTime.UtcNow
        };

        await _context.Set<NewsletterSubscription>()
            .AddAsync(subscription, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Subscribed successfully");
    }
}