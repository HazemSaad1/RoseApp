using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Notifications;
using RoseApp.Domain.Entities.Notifications;

namespace RoseApp.Application.Features.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryHandler
    : IRequestHandler<GetNotificationsQuery, ResponseViewModel<PaginationResult<NotificationDto>>>
{
    private readonly IDbContext _context;

    public GetNotificationsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<NotificationDto>>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Notification>()
            .AsNoTracking()
            .Where(n => n.UserId == request.UserId);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(n =>
                n.Title.Contains(request.Search) ||
                n.Message.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<NotificationDto>.Create(
            notifications, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<NotificationDto>>.Success(result);
    }
}