namespace TaskManagerMediatR.Application.Shared.Abstractions.Caching
{
    public interface ICacheVersionService
    {
        Task<long> Get(string cacheKey, CancellationToken cancellationToken = default);

        Task Increment(string cacheKey, CancellationToken cancellationToken = default);
    }
}
