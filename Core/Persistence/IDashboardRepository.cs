using Core.Models;
using FluentResults;

namespace Core.Persistence;

public interface IDashboardRepository
{
    public Task<AdminDashboard> GetAdminDashboard();
    public Task<Result<StudentDashboard>> GetStudentDashboard(string studentId);
}