using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Users.Commands.Logout
{
    public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        public LogoutCommandHandler(
            IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _refreshTokenService.Revoke(request.RefreshToken, cancellationToken);

            return Result.Success();
        }
    }
}
