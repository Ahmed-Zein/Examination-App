namespace Application.DTOs;

public class SubjectDto : CreateSubjectDto
{
    public int Id { get; set; }
    public List<QuestionDto> Questions { get; set; } = [];
}

public class CreateSubjectDto
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateSubjectDto : CreateSubjectDto
{
}