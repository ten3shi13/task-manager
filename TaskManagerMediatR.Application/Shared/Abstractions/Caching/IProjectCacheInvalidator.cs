namespace TaskManagerMediatR.Application.Shared.Abstractions.Caching
{
    public interface IProjectCacheInvalidator
    {
        Task InvalidateProject(Guid projectId, CancellationToken cancellationToken = default);
        Task InvalidateProjects(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
        Task InvalidateProjectWithProjects(IEnumerable<Guid> userIds, Guid projectId, CancellationToken cancellationToken = default);
    }
}
 