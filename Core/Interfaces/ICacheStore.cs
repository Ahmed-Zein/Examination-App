using FluentResults;

namespace Core.Interfaces;

public interface ICacheStore
{
    void Add<TItem>(ICacheKey<TItem> key, TItem item);

    Result<TItem> Get<TItem>(ICacheKey<TItem> key);
}