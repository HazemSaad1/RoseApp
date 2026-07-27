using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Notifications.Commands.MarkAllAsRead;

public record MarkAllAsReadCommand(Guid UserId)
    : IRequest<ResponseViewModel<string>>;