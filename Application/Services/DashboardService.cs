using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Persistence;
using FluentResults;

namespace Application.Services;

public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository = unitOfWork.DashboardRepository;

    public async Task<AdminDashboard> GetAdminDashboard()
    {
        return await _dashboardRepository.GetAdminDashboard();
    }

    public async Task<Result<StudentDashboard>> GetStudentDashboard(string studentId)
    {
        return await _dashboardRepository.GetStudentDashboard(studentId);
    }
}