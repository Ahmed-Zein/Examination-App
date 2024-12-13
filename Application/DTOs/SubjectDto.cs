namespace Application.DTOs;

public abstract class SubjectDtoBase
{
    public string Name { get; set; } = string.Empty;
}

public class SubjectDto : SubjectDtoBase
{
    public int Id { get; set; }
    public List<ExamDto> Exams { get; set; } = [];
    public List<QuestionDto> Questions { get; set; } = [];
}

public class CreateSubjectDto : SubjectDtoBase
{
}

public class UpdateSubjectDto : CreateSubjectDto
{
}