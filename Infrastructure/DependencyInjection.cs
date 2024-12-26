using Core.Entities;
using Core.Interfaces;
using Core.Persistence;
using Infrastructure.Data;
using Infrastructure.RabbitMQ;
using Infrastructure.Repositories;
using Infrastructure.Signalr;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MemoryCache = Infrastructure.Cache.MemoryCache;

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

        services.AddAppCache(configuration);
        services.AddAppDbContext(configuration, environment);
        services.AddAppIdentity();
        return services;
    }

    private static IServiceCollection AddAppCache(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheConfig = configuration.GetSection("Cache").GetChildren();
        var expirationTimeSpans =
            cacheConfig.ToDictionary(section => section.Key, section => TimeSpan.Parse(section.Value!));

        services.AddMemoryCache();
        services.AddSingleton<ICacheStore>(x => new MemoryCache(x.GetService<IMemoryCache>()!, expirationTimeSpans));
        return services;
    }

    private static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ??
                                 throw new InvalidOperationException("Missing DefaultConnection"));
            if (environment.IsDevelopment()) options.EnableSensitiveDataLogging();
        });
        return services;
    }

    private static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
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