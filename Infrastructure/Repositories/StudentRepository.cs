using Application.Interfaces.Persistence;
using Application.Models;
using Core.Constants;
using Core.Entities;
using FluentResults;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudentRepository(
    AppDbContext context,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager)
    : IStudentRepository
{
    public async Task<PagedData<AppUser>> GetAllAsync(PaginationQuery query)
    {
        var studentRole = await roleManager.Roles
            .Where(role => role.Name!.Equals(AuthRolesConstants.Student))
            .Select(role => role.Id).FirstOrDefaultAsync();

        var studentsQuery = context.Users
            .Join(context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, _) => user)
            .AsNoTracking();
        // // Used instead of writing two queries to get all students userIds then fetch them on another query 
        // var studentsQuery =
        //     from user in context.Users
        //     join userRole in context.UserRoles on user.Id equals userRole.UserId
        //     where userRole.RoleId == studentRole
        //     select user;

        return await PagedData<AppUser>.CreateAsync(studentsQuery, query);
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