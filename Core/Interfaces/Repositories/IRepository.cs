using FluentResults;

namespace Core.Repositories;

public interface IRepository<T, in TP>
{
    Task<bool> AnyAsync(TP id);

    Task<List<T>> GetAllAsync();
    Task<Result<T>> GetByIdAsync(int id);

    void Delete(T entity);
    Task AddAsync(T entity);
    Task<Result<T>> UpdateAsync(int id, T toUpdate);
}