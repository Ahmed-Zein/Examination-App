using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Persistence;

namespace Application.Services;

public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository = unitOfWork.DashboardRepository;

    public async Task<AdminDashboard> GetAdminDashboard()
    {
        return await _dashboardRepository.GetAdminDashboard();
    }
}