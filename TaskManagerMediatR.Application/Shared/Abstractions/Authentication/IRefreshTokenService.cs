using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Authentication
{
    public interface IRefreshTokenService
    {
        Task<(string RefreshToken, DateTime ExpiresAtUtc)> Issue(Guid userId, CancellationToken cancellationToken = default);
        Task<Result<(Guid UserId, string NewRefreshToken, DateTime ExpiresAtUtc)>> Rotate(string refreshToken, CancellationToken cancellationToken = default);
        Task Revoke(string refreshToken, CancellationToken cancellationToken = default);
    }
}
