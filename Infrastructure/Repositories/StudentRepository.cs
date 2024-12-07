using Application.Interfaces.Persistence;
using Core.Constants;
using Core.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class StudentRepository(UserManager<AppUser> userManager) : IStudentRepository
{
    public async Task<List<AppUser>> GetAllAsync()
    {
        var students = (List<AppUser>)await userManager.GetUsersInRoleAsync(AuthRolesConstants.Student);
        return students;
    }

    public async Task<Result<AppUser>> GetByIdAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        return user switch
        {
            null => Result.Fail<AppUser>("User not found"),
            _ => Result.Ok(user)
        };
    }
}