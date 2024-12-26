using Core.Interfaces;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache;

public class MemoryCache(IMemoryCache memoryCache, Dictionary<string, TimeSpan> expirationTimeSpans) : ICacheStore
{
    public void Add<TItem>(ICacheKey<TItem> key, TItem item)
    {
        if (!expirationTimeSpans.TryGetValue(item!.GetType().Name, out var expiresIn))
            expiresIn = expirationTimeSpans["Default"];

        memoryCache.Set(key.CacheKey, item, expiresIn);
    }

    public Result<TItem> Get<TItem>(ICacheKey<TItem> key)
    {
        return memoryCache.TryGetValue(key.CacheKey, out TItem? item)
            ? Result.Ok(item)!
            : Result.Fail<TItem>($"Key {key} not found");
    }
}