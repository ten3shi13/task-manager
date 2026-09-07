using Microsoft.Extensions.Options;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    public sealed class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtTokenOptions _jwtTokenOptions;
        private readonly ITokenHashService _tokenHashService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            ITokenHashService tokenHashService,
            IOptions<JwtTokenOptions> jwtTokenOptions,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _unitOfWork = unitOfWork;
            _tokenHashService = tokenHashService;
            _dateTimeProvider = dateTimeProvider;
            _jwtTokenOptions = jwtTokenOptions.Value;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<(string RefreshToken, DateTime ExpiresAtUtc)> Issue(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokenPlain = _tokenHashService.GenerateRefreshToken();
            var dateTimeNow = _dateTimeProvider.UtcNow;
            var expires = dateTimeNow.AddDays(_jwtTokenOptions.RefreshTokenExpirationDays);

            var tokenResult = RefreshToken.Create(
                Guid.NewGuid(),
                _tokenHashService.Hash(tokenPlain),
                userId,
                dateTimeNow,
                expires,
                Guid.NewGuid());

            await _refreshTokenRepository.Add(tokenResult.Value, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return (tokenPlain, expires);
        }

        public async Task Revoke(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return;

            var tokenHash = _tokenHashService.Hash(refreshToken);

            var existing = await _refreshTokenRepository.GetByHash(tokenHash, cancellationToken);
            if (existing is null || !existing.IsActive)
                return;

            existing.Revoke(_dateTimeProvider.UtcNow);
            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }

        public async Task<Result<(Guid UserId, string NewRefreshToken, DateTime ExpiresAtUtc)>> Rotate(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return Result.Failure<(Guid, string, DateTime)>(DomainErrors.RefreshToken.Invalid);

            var tokenHash = _tokenHashService.Hash(refreshToken);

            var existingToken = await _refreshTokenRepository.GetByHash(tokenHash, cancellationToken);

            if (existingToken is null)
                return Result.Failure<(Guid, string, DateTime)>(DomainErrors.RefreshToken.Invalid);

            if (existingToken.IsRevoked)
            {
                await RevokeFamily(existingToken.TokenFamilyId, cancellationToken);
                await _unitOfWork.CommitChangesAsync(cancellationToken);

                return Result.Failure<(Guid, string, DateTime)>(DomainErrors.RefreshToken.Reuse);
            }

            if (existingToken.IsExpired)
                return Result.Failure<(Guid, string, DateTime)>(DomainErrors.RefreshToken.Invalid);

            var newToken = _tokenHashService.GenerateRefreshToken();
            var newHash = _tokenHashService.Hash(newToken);

            var dateTimeNow = _dateTimeProvider.UtcNow;
            var newExpires = dateTimeNow.AddDays(_jwtTokenOptions.RefreshTokenExpirationDays);

            existingToken.Revoke(dateTimeNow, newHash);

            var replacementTokenResult = RefreshToken.Create(
                Guid.NewGuid(),
                newHash,
                existingToken.UserId,
                dateTimeNow,
                newExpires,
                existingToken.TokenFamilyId);

            await _refreshTokenRepository.Add(replacementTokenResult.Value);
            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success((existingToken.UserId, newToken, newExpires));
        }

        private async Task RevokeFamily(Guid tokenFamilyId, CancellationToken cancellationToken)
        {
            var dateTimeNow = _dateTimeProvider.UtcNow;

            var tokens = await _refreshTokenRepository.GetActiveByFamily(tokenFamilyId, cancellationToken);
            foreach (var token in tokens)
                token.Revoke(dateTimeNow);
        }

    }
}
