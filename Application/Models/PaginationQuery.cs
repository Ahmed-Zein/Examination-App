namespace Application.Models;

public class PaginationQuery
{
    public PaginationQuery()
    {
    }

    public int? Page { get; set; }
    public int? PageSize { get; set; }
}