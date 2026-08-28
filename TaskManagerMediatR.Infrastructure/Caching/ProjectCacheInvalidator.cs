using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.Infrastructure.Caching
{
    internal sealed class ProjectCacheInvalidator : IProjectCacheInvalidator
    {
        private readonly ICacheService _cache;
        private readonly ICacheVersionService _cacheVersionService;

        public ProjectCacheInvalidator(
            ICacheService cache,
            ICacheVersionService cacheVersionService)
        {
            _cache = cache;
            _cacheVersionService = cacheVersionService;
        }
        public Task InvalidateProject(Guid projectId, CancellationToken cancellationToken = default) =>
            _cache.Remove(CacheKeys.Project(projectId), cancellationToken);

        public async Task InvalidateProjects(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
        {
            foreach (var userId in userIds.Distinct())
            {
                await _cacheVersionService.Increment(CacheKeys.UserProjectsVersion(userId), cancellationToken);

            }
        }

        public async Task InvalidateProjectWithProjects(IEnumerable<Guid> userIds, Guid projectId, CancellationToken cancellationToken = default)
        {
            await InvalidateProject(projectId, cancellationToken);

            await InvalidateProjects(userIds, cancellationToken);
        }
    }
}
