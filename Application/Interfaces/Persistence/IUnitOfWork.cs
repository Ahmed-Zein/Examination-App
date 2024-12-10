
namespace Application.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
    ISubjectRepository SubjectRepository { get; }
    IStudentRepository StudentRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IAnswerRepository AnswerRepository { get; }
    IExamRepository ExamRepository { get; }
}