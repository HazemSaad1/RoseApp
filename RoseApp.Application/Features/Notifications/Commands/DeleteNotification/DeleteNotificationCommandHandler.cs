using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Notifications;

namespace RoseApp.Application.Features.Notifications.Commands.DeleteNotification;

public class DeleteNotificationCommandHandler
    : IRequestHandler<DeleteNotificationCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteNotificationCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await _context.Set<Notification>()
            .FirstOrDefaultAsync(n => n.Id == request.NotificationId
                && n.UserId == request.UserId, cancellationToken);

        if (notification is null)
            throw new NotFoundException(nameof(Notification), request.NotificationId);

        _context.Set<Notification>().Remove(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Notification deleted successfully");
    }
}