using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Contracts.Tasks;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetById
{
    public sealed record GetTaskQuery(Guid Id) : IQuery<TaskResponse>;
}
