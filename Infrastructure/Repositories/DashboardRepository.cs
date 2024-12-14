using Application.DTOs;
using Application.Interfaces.Persistence;
using Core.Constants;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DashboardRepository(AppDbContext context, RoleManager<IdentityRole> roleManager) : IDashboardRepository
{
    public async Task<AdminDashboard> GetAdminDashboard()
    {
        var totalSubjects = await context.Subjects.CountAsync();
        var totalExamResults = await context.ExamResults.CountAsync();

        var studentRole = await roleManager.Roles
            .Where(role => role.Name!.Equals(AuthRolesConstants.Student))
            .Select(role => role.Id).FirstOrDefaultAsync();

        var studentQuery = from user in context.Users
            join userRole in context.UserRoles on user.Id equals userRole.UserId
            where userRole.RoleId == studentRole
            select user.Id;
        var totalStudent = await studentQuery.CountAsync();

        return new AdminDashboard
        {
            TotalStudents = totalStudent,
            TotalSubjects = totalSubjects,
            TotalExamsTaken = totalExamResults,
        };
    }
}