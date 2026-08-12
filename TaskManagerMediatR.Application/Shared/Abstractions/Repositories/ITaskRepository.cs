using Task = TaskManagerMediatR.Domain.Models.Task;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Repositories
{
    public interface ITaskRepository
    {
        Task<Guid> Add(Task task, CancellationToken cancellationToken = default);
        Task<int> Delete(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Task>> Get(CancellationToken cancellationToken = default);
        Task<Task?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Task>> GetByProjectId(Guid projectId, CancellationToken cancellationToken = default);

    }
}