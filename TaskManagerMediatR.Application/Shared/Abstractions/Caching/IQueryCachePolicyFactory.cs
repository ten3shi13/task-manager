using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Caching
{
    public interface IQueryCachePolicyFactory
    {
        IQueryCachePolicy CreateForTasks();
        IQueryCachePolicy CreateForProjects();
    }
}
