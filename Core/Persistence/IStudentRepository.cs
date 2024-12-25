using Core.Entities;
using Core.Models;
using FluentResults;

namespace Core.Persistence;

public interface IStudentRepository
{
    Task<PagedData<AppUser>> GetAllAsync(PaginationQuery pagination);
    Task<bool> Exists(string userId);
    Task<Result<AppUser>> GetByIdAsync(string id);

    Task<Result<AppUser>> UpdateUserLock(string studentId);
}