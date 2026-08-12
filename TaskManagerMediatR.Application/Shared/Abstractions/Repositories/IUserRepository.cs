using TaskManagerMediatR.Domain.Models;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> Add(User user, CancellationToken cancellationToken = default);
        Task<int> Delete(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> Get(CancellationToken cancellationToken = default);
        Task<User?> GetById(Guid id, CancellationToken cancellationToken = default);
    }
}