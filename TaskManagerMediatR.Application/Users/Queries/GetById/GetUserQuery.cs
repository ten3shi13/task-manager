using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Contracts.Users;

namespace TaskManagerMediatR.Application.Users.Queries.GetById
{
    public sealed record GetUserQuery(Guid Id) : IQuery<UserResponse>;
}
