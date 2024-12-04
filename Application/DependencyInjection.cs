using Application.Interfaces;
using Application.Mappers;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IStudentServices, StudentServices>();
        services.AddAutoMapper(typeof(AppMappersProfiles));

        return services;
    }
}