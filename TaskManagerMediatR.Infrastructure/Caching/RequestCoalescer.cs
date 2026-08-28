using System.Collections.Concurrent;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;

namespace TaskManagerMediatR.Infrastructure.Caching
{
    internal sealed class RequestCoalescer : IRequestCoalescer
    {
        private readonly ConcurrentDictionary<string, Lazy<Task<object?>>> _inflight = new();

        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            CancellationToken cancellationToken = default)
        {
            var lazy = _inflight.GetOrAdd(
                key,
                _ => new Lazy<Task<object?>>(async () =>
                {
                    try
                    {
                        return await factory(cancellationToken).ConfigureAwait(false);
                    }
                    finally
                    {
                        _inflight.TryRemove(key, out var _);
                    }
                }));

            var result = await lazy.Value.ConfigureAwait(false);
            return (T)result!;
        }
    }
}
