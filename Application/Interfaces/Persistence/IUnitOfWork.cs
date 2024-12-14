namespace Application.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
    IExamRepository ExamRepository { get; }
    IAnswerRepository AnswerRepository { get; }
    ISubjectRepository SubjectRepository { get; }
    IStudentRepository StudentRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IDashboardRepository DashboardRepository { get; }
    IExamResultRepository ExamResultRepository { get; }
}