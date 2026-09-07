using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Contracts.Authentication;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Application.Users.Commands.Login
{
    public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, TokenResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
            IDateTimeProvider dateTimeProvider,
            IRefreshTokenService refreshTokenService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _dateTimeProvider = dateTimeProvider;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure)
                return Result.Failure<TokenResponse>(emailResult.Errors);

            var user = await _userRepository.GetByEmail(emailResult.Value.Value, cancellationToken);
            if(user is null)
                return Result.Failure<TokenResponse>(DomainErrors.User.InvalidCredentials);

            var dateTimeNow = _dateTimeProvider.UtcNow;

            if (user.IsLockedOut(dateTimeNow))
                return Result.Failure<TokenResponse>(DomainErrors.User.LockedOut);

            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                user.RegisterFailedAccess(dateTimeNow, maxAttempts: 5, TimeSpan.FromMinutes(15)); /////////

                await _unitOfWork.CommitChangesAsync(cancellationToken);
                return Result.Failure<TokenResponse>(DomainErrors.User.InvalidCredentials);
            }

            user.ResetAccessFailed();
            await _unitOfWork.CommitChangesAsync(cancellationToken);

            string accessToken = _jwtTokenService.GenerateAccessToken(user);

            DateTime accessExp = _jwtTokenService.AccessTokenExpiresAtUtc;
            var (refreshToken, refreshExp) = await _refreshTokenService.Issue(user.Id, cancellationToken);

            return Result.Success(new TokenResponse(accessToken, refreshToken, accessExp, refreshExp));
        }
    }
}
