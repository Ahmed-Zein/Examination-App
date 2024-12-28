using Application.Interfaces;
using Application.Mappers;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IStudentServices, StudentServices>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IEvaluationService, EvaluationService>();
        services.AddScoped<IExamResultService, ExamResultService>();

        services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddAutoMapper(typeof(AppMappersProfiles));
        services.AddValidatorsFromAssemblyContaining<CreateQuestionDtoValidator>();

        return services;
    }
}