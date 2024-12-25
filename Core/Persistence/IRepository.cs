using Core.Entities;
using FluentResults;

namespace Core.Persistence;

public interface IRepository<T> where T : BaseModel
{
    Task<bool> AnyAsync(int id);

    Task<List<T>> GetAllAsync();
    Task<Result<T>> GetByIdAsync(int id);

    void Delete(T entity);
    Task AddAsync(T entity);
    Task<Result<T>> UpdateAsync(int id, T toUpdate);
}