using Microsoft.AspNetCore.SignalR;
using RoseApp.API.Hubs;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Application.DTOs.Notifications;

namespace RoseApp.API.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationAsync(string userId, NotificationDto notification)
        => await _hubContext.Clients
            .Group(userId)
            .SendAsync("notification:new", notification);

    public async Task SendNotificationUpdatedAsync(string userId, NotificationDto notification)
        => await _hubContext.Clients
            .Group(userId)
            .SendAsync("notification:updated", notification);

    public async Task SendAllReadAsync(string userId)
        => await _hubContext.Clients
            .Group(userId)
            .SendAsync("notification:all_read");
}