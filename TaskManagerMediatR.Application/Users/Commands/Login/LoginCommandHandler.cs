using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Application.Users.Commands.Login
{
    public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
                return Result.Failure<string>(emailResult.Errors);

            var user = await _userRepository.GetByEmail(emailResult.Value.Value);

            if(user is null)
                return Result.Failure<string>(DomainErrors.User.InvalidCredentials);

            string accessToken = _jwtTokenService.GenerateAccessToken(user);

            return Result.Success(accessToken);
        }
    }
}
