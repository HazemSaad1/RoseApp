using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Subscriptions;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Subscriptions.Queries.GetSubscriptions;

public class GetSubscriptionsQueryHandler
    : IRequestHandler<GetSubscriptionsQuery, ResponseViewModel<PaginationResult<SubscriptionDto>>>
{
    private readonly IDbContext _context;

    public GetSubscriptionsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<SubscriptionDto>>> Handle(
        GetSubscriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<NewsletterSubscription>()
            .AsNoTracking()
            .OrderByDescending(s => s.SubscribedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var subscriptions = await query
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(s => new SubscriptionDto
            {
                Id = s.Id,
                Email = s.Email,
                SubscribedAt = s.SubscribedAt
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<SubscriptionDto>.Create(
            subscriptions, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<SubscriptionDto>>.Success(result);
    }
}