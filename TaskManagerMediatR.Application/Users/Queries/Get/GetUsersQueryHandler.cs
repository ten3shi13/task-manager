using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Contracts.Users;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Users.Queries.Get
{
    public sealed record GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IReadOnlyCollection<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<IReadOnlyCollection<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.Get(cancellationToken);

            return users.Select(u => new UserResponse(u.FirstName.Value, u.Email.Value, u.CreatedAt)).ToList().AsReadOnly();
        }
    }
}
