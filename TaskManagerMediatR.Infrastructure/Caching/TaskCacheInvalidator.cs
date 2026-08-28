using TaskManagerMediatR.Application.Shared.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;

namespace TaskManagerMediatR.Infrastructure.Caching
{
    internal sealed class TaskCacheInvalidator : ITaskCacheInvalidator
    {
        private readonly ICacheService _cache;
        private readonly ICacheVersionService _cacheVersionService;

        public TaskCacheInvalidator(
            ICacheService cache,
            ICacheVersionService cacheVersionService)
        {
            _cache = cache;
            _cacheVersionService = cacheVersionService;
        }
        public async Task InvalidateTask(Guid taskId, CancellationToken cancellationToken = default) =>
            await _cache.Remove(CacheKeys.Task(taskId), cancellationToken);

        public async Task InvalidateTasks(Guid projectId, CancellationToken cancellationToken = default)
        {
            await _cacheVersionService.Increment(CacheKeys.ProjectTasksVersion(projectId), cancellationToken);
        }

        public async Task InvalidateTaskWithProjectTasks(Guid projectId, Guid taskId, CancellationToken cancellationToken = default)
        {
            await InvalidateTask(taskId, cancellationToken);

            await InvalidateTasks(projectId, cancellationToken);
        }
    }
}
