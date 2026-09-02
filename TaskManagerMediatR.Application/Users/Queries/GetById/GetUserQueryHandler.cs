using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Contracts.Users;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Users.Queries.GetById
{
    public sealed record GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id, cancellationToken);

            if (user is null)
                return Result.Failure<UserResponse>(DomainErrors.User.NotFound(request.Id));

            return new UserResponse(user.FirstName.Value, user.Email.Value, user.CreatedAt);
        }
    }
}
