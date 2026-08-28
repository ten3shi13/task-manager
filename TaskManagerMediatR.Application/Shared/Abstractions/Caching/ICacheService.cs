namespace TaskManagerMediatR.Application.Shared.Abstractions.Caching
{
    public interface ICacheService
    {
        Task<T?> Get<T>(string cacheKey, CancellationToken cancellationToken = default)
            where T : class;

        Task Set<T> (string cacheKey, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
            where T : class;

        Task Remove(string cacheKey, CancellationToken cancellationToken = default);

        Task RemoveByPrefix(string cacheKey, CancellationToken cancellationToken = default);
    }
}
