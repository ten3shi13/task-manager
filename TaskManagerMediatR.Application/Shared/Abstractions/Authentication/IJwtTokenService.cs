using TaskManagerMediatR.Domain.Models;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);
        DateTime AccessTokenExpiresAtUtc { get; }
    }
}
