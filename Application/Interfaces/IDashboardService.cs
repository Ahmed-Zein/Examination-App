using Application.DTOs;

namespace Application.Interfaces;

public interface IDashboardService
{
    Task<AdminDashboard> GetAdminDashboard();
}