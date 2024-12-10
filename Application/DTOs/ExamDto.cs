namespace Application.DTOs;

public abstract class ExamDtoBase
{
    public string ModelName { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}

public class ExamDto : ExamDtoBase
{
    public int Id { get; set; }
}

public class CreateExamDto : ExamDtoBase
{
}

public class StudentExam : ExamDtoBase
{
    public int Id { get; set; }
    public List<StudentQuestion> Questions { get; set; } = [];
}

public class AddQuestionToExamDto
{
    public int ExamId { get; set; }
    public int SubjectId { get; set; }
    public List<int> QuestionIds { get; set; } = [];
}