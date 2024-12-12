using Core.Entities;
using FluentResults;

namespace Application.Interfaces.Persistence;

public interface IStudentRepository
{
    Task<List<AppUser>> GetAllAsync();
    Task<bool> Exists(string userId);
    Task<Result<AppUser>> GetByIdAsync(string id);

    Task<Result<AppUser>> UpdateUserLock(string studentId);
}