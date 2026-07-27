using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Notifications.Commands.ClearAllNotifications;

public record ClearAllNotificationsCommand(Guid UserId)
    : IRequest<ResponseViewModel<string>>;