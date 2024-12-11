namespace Application.DTOs;

public class SolutionsDto
{
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
}

public class ExamSolutionsDto
{
    public int ExamResultId { get; set; }
    public List<SolutionsDto> Solutions { get; set; } = [];
}