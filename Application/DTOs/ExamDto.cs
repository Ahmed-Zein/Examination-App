namespace Application.DTOs;

public class ExamDto : CreateExamDto
{
    public int Id { get; set; }
    public List<QuestionDto> Questions { get; set; } = [];
}

public class CreateExamDto
{
    public string ModelName { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}

public class AddQuestionToExamDto
{
    public int ExamId { get; set; }
    public int SubjectId { get; set; }
    public List<int> QuestionIds { get; set; } = [];
}