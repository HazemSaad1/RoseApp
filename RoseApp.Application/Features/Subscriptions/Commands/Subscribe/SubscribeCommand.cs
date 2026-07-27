using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Subscriptions.Commands.Subscribe;

public record SubscribeCommand(string Email)
    : IRequest<ResponseViewModel<string>>;