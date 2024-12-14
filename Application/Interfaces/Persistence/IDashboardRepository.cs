using Application.DTOs;

namespace Application.Interfaces.Persistence;

public interface IDashboardRepository
{
    public Task<AdminDashboard> GetAdminDashboard();
}