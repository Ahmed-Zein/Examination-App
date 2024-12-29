using Core.Interfaces;
using Core.Persistence;
using Infrastructure.Extensions;
using Infrastructure.RabbitMQ;
using Infrastructure.Repositories;
using Infrastructure.Signalr;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddSignalR();
        services.AddScoped<ISignalrClientContext, SignalrClientContext>();
        services.AddScoped<IClientSideNotification, IClientSideNotificationManager>();

        services.AddScoped<IRabbitConfig, RabbitConfig>();
        services.AddScoped<IRabbitPublisher, RabbitPublisher>();
        services.AddScoped<IRabbitConsumer, RabbitEvaluationConsumer>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAdminNotificationRepository, AdminNotificationRepository>();
        services.AddScoped<IStudentNotificationRepository, StudentNotificationRepository>();

        services.AddMongoDb(configuration);
        services.AddAppCache(configuration);
        services.AddAppDbContext(configuration, environment);
        services.AddAppIdentity();
        return services;
    }
}