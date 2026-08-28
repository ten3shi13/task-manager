using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.Application.Shared
{
    public static class QueryClassifier
    {
        public static CacheQueryType Classify<T>(T query, Func<T, bool> hasSearch, Func<T, bool> isDefault)
        {
            if (hasSearch(query))
                return CacheQueryType.Search;

            return isDefault(query) ? CacheQueryType.Default : CacheQueryType.Filtered;
        }
    }
}
