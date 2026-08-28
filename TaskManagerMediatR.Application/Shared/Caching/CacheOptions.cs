namespace TaskManagerMediatR.Application.Shared.Caching
{
    public sealed class CacheOptions
    {
        public TimeSpan DefaultExpiration { get; init; }
        public TimeSpan ProjectExpiration { get; init; }
        public TimeSpan TaskExpiration { get; init; }
        public QueryCacheOptions Tasks { get; init; } = new();
        public QueryCacheOptions Projects { get; init; } = new();
    }

    public sealed class QueryCacheOptions
    {
        public TimeSpan DefaultExpiration { get; init; }
        public TimeSpan FilteredExpiration { get; init; }
        public TimeSpan SearchExpiration { get; init; }
    }
}
