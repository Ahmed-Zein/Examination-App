namespace Core.Interfaces;

public interface ICacheKey<TItem>
{
    string CacheKey { get; }
}