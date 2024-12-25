namespace Core.Models;

public class PagedData<T>(List<T> data, int page, int pageSize, int totalCount)
{
    public int Page { get; } = page;
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = totalCount;
    public List<T> Data { get; } = data;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}