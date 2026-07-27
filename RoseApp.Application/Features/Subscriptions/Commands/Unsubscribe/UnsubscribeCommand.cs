using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Subscriptions.Commands.Unsubscribe;

public record UnsubscribeCommand(string Email)
    : IRequest<ResponseViewModel<string>>;