namespace Application.QueryParameters;

public class SubjectQueryParameters : QueryParametersBase
{
    public bool IncludeQuestions { get; set; } = false;
}

public abstract class QueryParametersBase
{
    public SortDirection SortDirection = SortDirection.Ascending;
}