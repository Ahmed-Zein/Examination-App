using Core.Models;
using FluentResults;

namespace Application.Interfaces;

public interface IDashboardService
{
    Task<AdminDashboard> GetAdminDashboard();
    Task<Result<StudentDashboard>> GetStudentDashboard(string studentId);
}