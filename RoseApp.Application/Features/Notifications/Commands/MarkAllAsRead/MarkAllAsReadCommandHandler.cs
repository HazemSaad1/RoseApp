using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Domain.Entities.Notifications;

namespace RoseApp.Application.Features.Notifications.Commands.MarkAllAsRead;

public class MarkAllAsReadCommandHandler
    : IRequestHandler<MarkAllAsReadCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;
    private readonly INotificationService _notificationService;

    public MarkAllAsReadCommandHandler(
        IDbContext context,
        INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<ResponseViewModel<string>> Handle(
        MarkAllAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var notifications = await _context.Set<Notification>()
            .Where(n => n.UserId == request.UserId && !n.IsRead)
            .ToListAsync(cancellationToken);

        if (!notifications.Any())
            return ResponseViewModel<string>.Success("No unread notifications");

        foreach (var notification in notifications)
            notification.IsRead = true;

        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.SendAllReadAsync(request.UserId.ToString());

        return ResponseViewModel<string>.Success("All notifications marked as read");
    }
}