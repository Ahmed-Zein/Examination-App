namespace Core.Persistence;

public interface IUnitOfWork : IDisposable
{
    IExamRepository ExamRepository { get; }
    IAnswerRepository AnswerRepository { get; }
    ISubjectRepository SubjectRepository { get; }
    IStudentRepository StudentRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IDashboardRepository DashboardRepository { get; }
    IExamResultRepository ExamResultRepository { get; }
    Task CommitAsync();
}