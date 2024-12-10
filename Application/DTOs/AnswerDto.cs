namespace Application.DTOs;

public abstract class AnswerBaseDto
{
    public string Text { get; set; } = string.Empty;
}

public class AnswerDto : AnswerBaseDto
{
    public int Id { get; set; }
    public bool IsCorrect { get; set; }
}

public class CreateAnswerDto : AnswerBaseDto
{
    public bool IsCorrect { get; set; }
}

public class StudentAnswer : AnswerBaseDto
{
    public int Id { get; set; }
}