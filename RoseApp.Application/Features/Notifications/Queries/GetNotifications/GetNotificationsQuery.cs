using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Notifications;

namespace RoseApp.Application.Features.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery(
    Guid UserId,
    int Page = 1,
    int Limit = 20,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<NotificationDto>>>;