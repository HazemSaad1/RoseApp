using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Application.DTOs.Notifications;
using RoseApp.Domain.Entities.Notifications;

namespace RoseApp.Application.Features.Notifications.Commands.CreateNotification;

public class CreateNotificationCommandHandler
    : IRequestHandler<CreateNotificationCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;
    private readonly INotificationService _notificationService;

    public CreateNotificationCommandHandler(
        IDbContext context,
        INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Message = request.Message,
            IsRead = false
        };

        await _context.Set<Notification>().AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.SendNotificationAsync(
            request.UserId.ToString(),
            new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            });

        return ResponseViewModel<string>.Success(
            "Notification sent successfully", "Created", 201);
    }
}