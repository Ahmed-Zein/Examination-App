using Application.DTOs;
using Application.Interfaces.Persistence;
using Core.Constants;
using Core.Enums;
using FluentResults;
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
            TotalExamsTaken = totalExamResults
        };
    }

    public async Task<Result<StudentDashboard>> GetStudentDashboard(string studentId)
    {
        var student = await context.Users.FindAsync(studentId);
        if (student is null)
            return Result.Fail("Student not found");

        var totalExamResults = await context.ExamResults.Where(exam => exam.AppUserId == studentId).CountAsync();
        var totalExamsInEvaluation = await context.ExamResults
            .Where(exam => exam.Status == ExamResultStatus.InEvaluation && exam.AppUserId == studentId).CountAsync();
        var totalExamPassed = await context.ExamResults
            .Where(exam => exam.StudentScore > (decimal)(exam.TotalScore * 0.60) && exam.AppUserId == studentId)
            .CountAsync();
        return Result.Ok(
            new StudentDashboard()
            {
                Name = $"{student.FirstName} {student.LastName}",
                TotalExams = totalExamResults,
                TotalExamsInEvaluation = totalExamsInEvaluation,
                PassedExams = totalExamPassed
            });
    }
}