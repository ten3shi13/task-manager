using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Caching
{
    public interface IQueryCachePolicy
    {
        bool ShouldCache(CacheQueryType queryType);

        TimeSpan GetExpiration(CacheQueryType queryType);
    }
}
