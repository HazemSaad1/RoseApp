using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Subscriptions.Commands.DeleteSubscription;

public record DeleteSubscriptionCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;