using FluentResults;

namespace Application.Interfaces.Persistence;

public interface IRepository<T, in TP>
{
    Task<bool> AnyAsync(TP id);

    Task<List<T>> GetAllAsync();
    Task<Result<T>> GetByIdAsync(TP id);

    void Delete(T entity);
    Task AddAsync(T entity);
    Task<Result<T>> UpdateAsync(TP id, T toUpdate);
}