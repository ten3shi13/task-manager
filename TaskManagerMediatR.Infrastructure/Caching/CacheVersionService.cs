using StackExchange.Redis;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;

namespace TaskManagerMediatR.Infrastructure.Caching
{
    public sealed class CacheVersionService : ICacheVersionService
    {
        private readonly IDatabase _database;

        public CacheVersionService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<long> Get(string cacheKey, CancellationToken cancellationToken = default)
        {
            var value = await _database.StringGetAsync(cacheKey);

            return value.HasValue ? (long)value : 0L;
        }

        public async Task Increment(string cacheKey, CancellationToken cancellationToken = default) =>
            await _database.StringIncrementAsync(cacheKey);
    }
}
