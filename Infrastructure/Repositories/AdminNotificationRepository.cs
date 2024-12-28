using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class AdminNotificationRepository(NotificationDbContext context) : IAdminNotificationRepository
{
    private const int MaxNotificationCount = 100;
    private readonly DateTime _notificationCutOffDate = DateTime.Now - TimeSpan.FromDays(30);

    public async Task<AdminNotification> AddAsync(AdminNotification notification)
    {
        await context.AdminNotifications.InsertOneAsync(notification);
        return notification;
    }

    public async Task<List<AdminNotification>> GetAllAsync()
    {
        var filter =
            Builders<AdminNotification>.Filter.Gt(notification => notification.CreatedAt, _notificationCutOffDate);
        var notifications = await context.AdminNotifications.Find(filter)
            .Limit(MaxNotificationCount).ToListAsync() ?? [];
        return notifications;
    }
}