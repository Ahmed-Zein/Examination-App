using Microsoft.EntityFrameworkCore;

namespace Core.Models;

public class PagedData<T>
{
    private PagedData(List<T> data, int page, int pageSize, int totalCount)
    {
        Data = data;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public List<T> Data { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

    public static async Task<PagedData<T>> CreateAsync(IQueryable<T> query, PaginationQuery pagination)
    {
        var pageSize = Math.Max(1, pagination.PageSize ?? 10);
        var page = Math.Max(1, pagination.Page ?? 1);

        var totalCount = await query.CountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedData<T>(data, page, pageSize, totalCount);
    }
}