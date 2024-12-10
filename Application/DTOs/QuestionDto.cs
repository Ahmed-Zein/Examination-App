namespace Application.DTOs;

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<AnswerDto> Answers { get; set; } = [];
}

public class AnswerDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

public class CreateQuestionDto
{
    public string Text { get; set; } = string.Empty;
    public List<CreateAnswerDto> Answers { get; set; } = [];
}

public class CreateAnswerDto
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}