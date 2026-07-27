using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Notifications.Commands.CreateNotification;

public record CreateNotificationCommand(
    Guid UserId,
    string Title,
    string Message) : IRequest<ResponseViewModel<string>>;