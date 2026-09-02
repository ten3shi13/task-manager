using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Contracts.Users;

namespace TaskManagerMediatR.Application.Users.Queries.Get
{
    public sealed record GetUsersQuery : IQuery<IReadOnlyCollection<UserResponse>>;
}
