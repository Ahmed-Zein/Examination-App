using Core.Models;

namespace Core.Persistence;

public interface IPaginationDataBuilder<T>
{
    IPaginationDataBuilder<T> WithPagination(PaginationQuery pagination);
    IPaginationDataBuilder<T> WithQueryable(IQueryable<T> query);
    Task<PagedData<T>> Build();
}