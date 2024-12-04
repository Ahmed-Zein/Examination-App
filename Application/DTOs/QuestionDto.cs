namespace Application.DTOs;

public class QuestionDto
{
    public string Text { get; set; } = string.Empty;
    public List<AnswerDto> Answers { get; set; } = [];
}

public class AnswerDto
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}