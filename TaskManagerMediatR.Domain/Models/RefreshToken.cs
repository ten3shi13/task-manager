using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.Models
{
    public class RefreshToken : Entity
    {
        private RefreshToken() { }
        private RefreshToken(
            Guid id,
            string tokenHash,
            Guid userId,
            DateTime createdAtUtc,
            DateTime expiresAtUtc,
            Guid tokenFamilyId) : base(id)
        {
            TokenHash = tokenHash;
            UserId = userId;
            CreatedAtUtc = createdAtUtc;
            ExpiresAtUtc = expiresAtUtc;
            TokenFamilyId = tokenFamilyId;
            RevokedAtUtc = null;
            ReplacedByTokenHash = null;
        }
        public static Result<RefreshToken> Create(
            Guid id,
            string tokenHash,
            Guid userId,
            DateTime createdAtUtc,
            DateTime expiresAtUtc,
            Guid tokenFamilyId)
        {
            return new RefreshToken(
                id,
                tokenHash,
                userId,
                createdAtUtc,
                expiresAtUtc,
                tokenFamilyId);
        }

        public string TokenHash { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }
        public DateTime? RevokedAtUtc { get; private set; }

        public string? ReplacedByTokenHash { get; private set; }

        public Guid TokenFamilyId { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
        public bool IsRevoked => RevokedAtUtc is not null;
        public bool IsActive => !IsRevoked && !IsExpired;

        public Result Revoke(DateTime dateTimeNow, string? replacedByTokenHash = null)
        {
            if (IsRevoked)
                return Result.Success();

            RevokedAtUtc = dateTimeNow;
            ReplacedByTokenHash = replacedByTokenHash;
            return Result.Success();
        }
    }
}
