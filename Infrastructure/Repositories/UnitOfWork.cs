using Core.Entities;
using Core.Persistence;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    : IUnitOfWork
{
    private IAnswerRepository? _answerRepository;
    private IDashboardRepository? _dashboardRepository;
    private IExamRepository? _examRepository;
    private IExamResultRepository? _examResultRepository;
    private IQuestionRepository? _questionRepository;
    private IStudentRepository? _studentRepository;
    private ISubjectRepository? _subjectRepository;

    public IExamRepository ExamRepository => _examRepository ??= new ExamRepository(context);
    public IAnswerRepository AnswerRepository => _answerRepository ??= new AnswerRepository(context);
    public ISubjectRepository SubjectRepository => _subjectRepository ??= new SubjectRepository(context);
    public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(context);

    public IExamResultRepository ExamResultRepository => _examResultRepository ??=
        new ExamResultRepository(context, new PaginationDataBuilder<ExamResult>());

    public IStudentRepository StudentRepository => _studentRepository ??=
        new StudentRepository(context, userManager, new PaginationDataBuilder<AppUser>());

    public IDashboardRepository DashboardRepository =>
        _dashboardRepository ??= new DashboardRepository(context, roleManager);

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
        userManager.Dispose();
    }
}