using Application.Interfaces.Persistence;
using Core.Constants;
using Core.Entities;
using FluentResults;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudentRepository(AppDbContext context, UserManager<AppUser> userManager) : IStudentRepository
{
    public async Task<List<AppUser>> GetAllAsync()
    {
        var students = (List<AppUser>)await userManager.GetUsersInRoleAsync(AuthRolesConstants.Student);
        return students;
    }

    public async Task<bool> Exists(string userId)
    {
        return await userManager.Users.AnyAsync(x => x.Id == userId);
    }

    public async Task<Result<AppUser>> GetByIdAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
            return Result.Fail<AppUser>("User not found");

        var userExamResults = await context.ExamResults.Where(e => e.AppUserId == user.Id).ToListAsync();
        user.ExamResults = userExamResults;
        return Result.Ok(user);
    }

    public async Task<Result<AppUser>> UpdateUserLock(string studentId)
    {
        var studentResult = await GetByIdAsync(studentId);
        if (!studentResult.IsSuccess)
            return studentResult.ToResult();

        var stu = studentResult.Value!;
        stu.isLocked = !stu.isLocked;

        return Result.Ok(stu);
    }
}