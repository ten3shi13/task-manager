using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Caching
{
    public interface ITaskCacheInvalidator
    {
       Task InvalidateTask(Guid taskId, CancellationToken cancellationToken = default);
       Task InvalidateTasks(Guid projectId, CancellationToken cancellationToken = default);
       Task InvalidateTaskWithProjectTasks(Guid projectId, Guid taskId, CancellationToken cancellationToken = default);
    }
}
