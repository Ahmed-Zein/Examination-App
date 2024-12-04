using Core.Entities;
using FluentResults;

namespace Core.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<List<AppUser>> GetAllAsync();
    Task<Result<AppUser>> GetByIdAsync(string id);
}