using TaskManagerMediatR.Application.Shared.Abstractions.Caching;

namespace TaskManagerMediatR.Application.Shared.Caching
{
    public sealed class QueryCachePolicy(QueryCacheOptions options) : IQueryCachePolicy
    {
        public bool ShouldCache(CacheQueryType queryType) => 
            queryType != CacheQueryType.Search;

        public TimeSpan GetExpiration(CacheQueryType queryType)
        {
            return queryType switch
            {
                CacheQueryType.Default => options.DefaultExpiration,
                CacheQueryType.Filtered => options.FilteredExpiration,
                CacheQueryType.Search => options.SearchExpiration,

                _ => throw new ArgumentOutOfRangeException(nameof(queryType), queryType, null)
            };
        }
    }
}
