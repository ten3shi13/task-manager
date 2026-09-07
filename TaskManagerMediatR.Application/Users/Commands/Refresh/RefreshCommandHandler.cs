using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Contracts.Authentication;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Users.Commands.Refresh
{
    public sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand, TokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshCommandHandler(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
        }


        public async Task<Result<TokenResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var rotated = await _refreshTokenService.Rotate(request.RefreshToken, cancellationToken);
            if (rotated.IsFailure)
                return Result.Failure<TokenResponse>(rotated.Errors);

            var (userId, newRefreshToken, refreshTokenExp) = rotated.Value;

            var user = await _userRepository.GetById(userId, cancellationToken);
            if (user is null)
                return Result.Failure<TokenResponse>(DomainErrors.RefreshToken.Invalid);

            var accessToken = _jwtTokenService.GenerateAccessToken(user);

            return Result.Success(new TokenResponse(accessToken, newRefreshToken, _jwtTokenService.AccessTokenExpiresAtUtc, refreshTokenExp));
        }
    }
}
