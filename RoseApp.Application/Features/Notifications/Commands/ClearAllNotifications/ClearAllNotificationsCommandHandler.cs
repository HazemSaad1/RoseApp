using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Notifications;

namespace RoseApp.Application.Features.Notifications.Commands.ClearAllNotifications;

public class ClearAllNotificationsCommandHandler
    : IRequestHandler<ClearAllNotificationsCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public ClearAllNotificationsCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        ClearAllNotificationsCommand request,
        CancellationToken cancellationToken)
    {
        var notifications = await _context.Set<Notification>()
            .Where(n => n.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        if (!notifications.Any())
            return ResponseViewModel<string>.Success("No notifications to clear");

        _context.Set<Notification>().RemoveRange(notifications);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("All notifications cleared successfully");
    }
}