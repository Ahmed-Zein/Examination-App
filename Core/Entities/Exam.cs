namespace Core.Entities;

public class Exam : BaseModel
{
    public TimeSpan Duration { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public List<ExamResult> ExamResults { get; set; } = [];
    public List<ExamQuestion> ExamQuestions { get; set; } = [];
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public List<Question> Questions { get; set; } = [];
}