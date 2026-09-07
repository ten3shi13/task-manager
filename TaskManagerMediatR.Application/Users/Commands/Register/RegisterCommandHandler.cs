using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Application.Users.Commands.Register
{
    public sealed record RegisterCommandHandler : ICommandHandler<RegisterCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure)
                return Result.Failure<Guid>(emailResult.Errors);

            var nameResult = FirstName.Create(request.FirstName);
            if (nameResult.IsFailure)
                return Result.Failure<Guid>(nameResult.Errors);

            if (await _userRepository.ExistsByEmail(emailResult.Value.Value, cancellationToken))
                return Result.Failure<Guid>(DomainErrors.User.EmailAlreadyInUse);

            if (string.IsNullOrEmpty(request.Password))
                return Result.Failure<Guid>(DomainErrors.User.EmptyPassword);

            var passwordHash = _passwordHasher.Hash(request.Password);

            var userResult = User.Create(Guid.NewGuid(), nameResult.Value, emailResult.Value, passwordHash);
            if (userResult.IsFailure)
                return Result.Failure<Guid>(userResult.Errors);

            await _userRepository.Add(userResult.Value, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success(userResult.Value.Id);
        }
    }
}
