using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Persistence;
using FluentResults;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudentRepository(
    AppDbContext context,
    UserManager<AppUser> userManager)
    : IStudentRepository
{
    public async Task<PagedData<AppUser>> GetAllAsync(PaginationQuery query)
    {
        var studentsQuery =
            context.Roles.Where(role => role.Name!.Equals(AuthRolesConstants.Student)).Join(context.UserRoles,
                    role => role.Id, userRole => userRole.RoleId, (role, userRoles) => userRoles)
                .Join(context.Users, userRole => userRole.UserId, user => user.Id, (userRole, user) => user)
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
        var user = await context.Users
            .Include(user => user.ExamResults.OrderByDescending(e => e.StartTime))
            .FirstOrDefaultAsync(user => user.Id == id);

        return user is not null ? Result.Ok(user) : Result.Fail<AppUser>(["User not found", ErrorType.NotFound]);
    }

    public async Task<Result<AppUser>> UpdateUserLock(string studentId)
    {
        var studentResult = await GetByIdAsync(studentId);
        if (!studentResult.IsSuccess)
            return studentResult.ToResult();

        var stu = studentResult.Value!;
        stu.IsLocked = !stu.IsLocked;

        return Result.Ok(stu);
    }
}