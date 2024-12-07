using Core.Entities;
using FluentResults;

namespace Application.Interfaces.Persistence;

public interface IStudentRepository
{
    Task<List<AppUser>> GetAllAsync();
    Task<Result<AppUser>> GetByIdAsync(string id);
}