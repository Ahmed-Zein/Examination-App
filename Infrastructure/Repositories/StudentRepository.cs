using Core.Entities;
using Core.Interfaces.Repositories;
using FluentResults;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

// TODO:
public class StudentRepository(AppDbContext context) : IStudentRepository
{
    public Task<bool> AnyAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<AppUser>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<AppUser>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Delete(AppUser entity)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(AppUser entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AppUser>> UpdateAsync(int id, AppUser toUpdate)
    {
        throw new NotImplementedException();
    }
}