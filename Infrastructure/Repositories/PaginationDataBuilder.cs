using Core.Models;
using Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaginationDataBuilder<T> : IPaginationDataBuilder<T>
{
    private PaginationQuery _pagination = new();
    private IQueryable<T>? _query;

    public IPaginationDataBuilder<T> WithQueryable(IQueryable<T> query)
    {
        _query = query;
        return this;
    }

    public IPaginationDataBuilder<T> WithPagination(PaginationQuery pagination)
    {
        _pagination = pagination;
        return this;
    }

    public async Task<PagedData<T>> Build()
    {
        if (_query is null)
            throw new NullReferenceException("Missing Query in PaginationDataBuilder");

        var pageSize = Math.Max(1, _pagination.PageSize ?? 10);
        var page = Math.Max(1, _pagination.Page ?? 1);

        var totalCount = await _query.CountAsync();
        var data = await _query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedData<T>(data, page, pageSize, totalCount);
    }
}