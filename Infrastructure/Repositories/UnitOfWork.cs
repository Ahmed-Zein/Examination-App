using Application.Interfaces.Persistence;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    : IUnitOfWork
{
    private IExamRepository? _examRepository;
    private IAnswerRepository? _answerRepository;
    private ISubjectRepository? _subjectRepository;
    private IStudentRepository? _studentRepository;
    private IQuestionRepository? _questionRepository;
    private IDashboardRepository? _dashboardRepository;
    private IExamResultRepository? _examResultRepository;

    public IExamRepository ExamRepository => _examRepository ??= new ExamRepository(context);
    public IAnswerRepository AnswerRepository => _answerRepository ??= new AnswerRepository(context);
    public ISubjectRepository SubjectRepository => _subjectRepository ??= new SubjectRepository(context);
    public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(context);
    public IExamResultRepository ExamResultRepository => _examResultRepository ??= new ExamResultRepository(context);
    public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(context, userManager);

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