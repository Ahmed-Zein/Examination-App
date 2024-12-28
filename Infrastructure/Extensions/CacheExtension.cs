using Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MemoryCache = Infrastructure.Cache.MemoryCache;

namespace Infrastructure.Extensions;

public static class CacheExtension
{
    public static IServiceCollection AddAppCache(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheConfig = configuration.GetSection("Cache").GetChildren();
        var expirationTimeSpans =
            cacheConfig.ToDictionary(section => section.Key, section => TimeSpan.Parse(section.Value!));

        services.AddMemoryCache();
        services.AddSingleton<ICacheStore>(x => new MemoryCache(x.GetService<IMemoryCache>()!, expirationTimeSpans));
        return services;
    }
}