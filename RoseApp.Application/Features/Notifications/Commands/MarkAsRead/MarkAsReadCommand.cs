using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Notifications.Commands.MarkAsRead;

public record MarkAsReadCommand(
    Guid NotificationId,
    Guid UserId) : IRequest<ResponseViewModel<string>>;