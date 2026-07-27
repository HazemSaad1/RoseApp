using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Notifications.Commands.DeleteNotification;

public record DeleteNotificationCommand(
    Guid NotificationId,
    Guid UserId) : IRequest<ResponseViewModel<string>>;