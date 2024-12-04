namespace Core.Entities;

public class Question : BaseModel
{
    public string Text { get; set; } = string.Empty;
    public List<Answer> Answers { get; set; } = [];
    public List<ExamQuestion> ExamQuestions { get; set; } = [];

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
}

public class Answer : BaseModel
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }
    public Question Question { get; set; }
}