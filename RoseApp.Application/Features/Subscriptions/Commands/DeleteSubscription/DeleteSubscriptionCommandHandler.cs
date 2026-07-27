using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler
    : IRequestHandler<DeleteSubscriptionCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteSubscriptionCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var subscription = await _context.Set<NewsletterSubscription>()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (subscription is null)
            throw new NotFoundException(nameof(NewsletterSubscription), request.Id);

        _context.Set<NewsletterSubscription>().Remove(subscription);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Subscription deleted successfully");
    }
}