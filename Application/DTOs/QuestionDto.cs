namespace Application.DTOs;

public abstract class QuestionBaseDto
{
    public string Text { get; set; } = string.Empty;
}

public class QuestionDto : QuestionBaseDto
{
    public int Id { get; set; }
    public List<AnswerDto> Answers { get; set; } = [];
}

public class StudentQuestion : QuestionBaseDto
{
    public int Id { get; set; }
    public List<StudentAnswer> Answers { get; set; } = [];
}

public class CreateQuestionDto : QuestionBaseDto
{
    public List<CreateAnswerDto> Answers { get; set; } = [];
}