using TaskManagerMediatR.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByHash(string tokenHash, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RefreshToken>> GetActiveByFamily(Guid tokenFamilyId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RefreshToken>> GetActiveByUser(Guid userId, CancellationToken cancellationToken = default);

        Task Add(RefreshToken token, CancellationToken cancellationToken = default);
    }
}
