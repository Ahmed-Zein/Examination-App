namespace Core.Models;

public class SortingQuery
{
    public string? OrderBy { get; set; } = string.Empty;
    public bool? Ascending { get; set; }
}