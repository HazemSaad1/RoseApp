using RoseApp.Application.DTOs.Notifications;

namespace RoseApp.Application.Contracts;

public interface INotificationService
{
    Task SendNotificationAsync(string userId, NotificationDto notification);
    Task SendNotificationUpdatedAsync(string userId, NotificationDto notification);
    Task SendAllReadAsync(string userId);
}