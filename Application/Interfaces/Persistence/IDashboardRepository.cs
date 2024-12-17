using Application.DTOs;
using FluentResults;

namespace Application.Interfaces.Persistence;

public interface IDashboardRepository
{
    public Task<AdminDashboard> GetAdminDashboard();
    public Task<Result<StudentDashboard>> GetStudentDashboard(string studentId);
}