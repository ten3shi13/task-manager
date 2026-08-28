namespace TaskManagerMediatR.Application.Shared.Abstractions.Caching
{
    public interface IRequestCoalescer
    {
        Task<T> GetOrCreateAsync<T>(string cacheKey, Func<CancellationToken, Task<T>> factory, CancellationToken cancellationToken = default);
    }
}
