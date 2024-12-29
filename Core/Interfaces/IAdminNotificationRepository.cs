using Core.Entities;

namespace Core.Interfaces;

public interface IAdminNotificationRepository
{
    Task<AdminNotification> AddAsync(AdminNotification notification);
    Task<List<AdminNotification>> GetAllAsync();
}