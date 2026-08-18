using TaskManagerMediatR.Domain.Models;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Repositories
{
    public interface IProjectRepository
    {
        Task<Guid> Add(Project project, CancellationToken cancellationToken = default);
        Task<int> Delete(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Project>> Get(CancellationToken cancellationToken = default);
        Task<Project?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<ProjectMember?> GetMember(Guid id, Guid userId, CancellationToken cancellationToken = default);
    }
}