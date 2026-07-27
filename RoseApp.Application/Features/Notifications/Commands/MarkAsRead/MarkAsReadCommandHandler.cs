using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Application.DTOs.Notifications;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Notifications;

namespace RoseApp.Application.Features.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommandHandler
    : IRequestHandler<MarkAsReadCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;
    private readonly INotificationService _notificationService;

    public MarkAsReadCommandHandler(
        IDbContext context,
        INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<ResponseViewModel<string>> Handle(
        MarkAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await _context.Set<Notification>()
            .FirstOrDefaultAsync(n => n.Id == request.NotificationId
                && n.UserId == request.UserId, cancellationToken);

        if (notification is null)
            throw new NotFoundException(nameof(Notification), request.NotificationId);

        notification.IsRead = true;
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.SendNotificationUpdatedAsync(
            request.UserId.ToString(),
            new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            });

        return ResponseViewModel<string>.Success("Notification marked as read");
    }
}