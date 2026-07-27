using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Subscriptions;

namespace RoseApp.Application.Features.Subscriptions.Queries.GetSubscriptions;

public record GetSubscriptionsQuery(int Page = 1, int Limit = 20)
    : IRequest<ResponseViewModel<PaginationResult<SubscriptionDto>>>;