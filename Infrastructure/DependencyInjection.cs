using Application.Interfaces.Persistence;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.RabbitMQ;
using Infrastructure.Repositories;
using Infrastructure.Signalr;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddSignalR();
        services.AddScoped<ISignalrClientContext, SignalrClientContext>();
        services.AddScoped<IClientNotificationInitiator, ClientNotificationManager>();

        services.AddScoped<IRabbitConfig, RabbitConfig>();
        services.AddScoped<IRabbitPublisher, RabbitPublisher>();
        services.AddScoped<IRabbitConsumer, RabbitEvaluationConsumer>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ??
                                 throw new InvalidOperationException("Missing DefaultConnection"));
            if (environment.IsDevelopment()) options.EnableSensitiveDataLogging();
        });

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        return services;
    }
}