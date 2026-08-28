using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using TaskManagerMediatR.Application.Shared.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;

namespace TaskManagerMediatR.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly CacheOptions _options;
        private readonly IDistributedCache _distributedCache;

        private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

        public RedisCacheService(
            IOptions<CacheOptions> options,
            IDistributedCache distributedCache)
        {
            _options = options.Value;
            _distributedCache = distributedCache;
        }

        public async Task<T?> Get<T>(string cacheKey, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                string? cachedValue = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

                return cachedValue is null ? default : JsonSerializer.Deserialize<T>(cachedValue);
            }
            catch (Exception)
            {
                return default;
            }
        }
        
        public async Task Set<T>(string cacheKey, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
        {
            try {
                string cacheValue = JsonSerializer.Serialize(value);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        expiration ?? _options.DefaultExpiration

                };

                await _distributedCache.SetStringAsync(cacheKey, cacheValue, options, cancellationToken);
            }
            catch (Exception)
            {
                return;
            }

            //CacheKeys.TryAdd(cacheKey, false);
        }

        public async Task Remove(string cacheKey, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(cacheKey, cancellationToken);

            //CacheKeys.TryRemove(cacheKey, out bool _);
        }

        public async Task RemoveByPrefix(string prefixKey, CancellationToken cancellationToken = default)
        {
            IEnumerable<Task> tasks = CacheKeys.Keys
                .Where(k => k.StartsWith(prefixKey))
                .Select(k => Remove(k, cancellationToken));

            await Task.WhenAll(tasks);
        }

    }
}
