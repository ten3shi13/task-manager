using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Contracts.Tasks;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetTasksByProject
{
    public sealed record GetTasksByProjectQuery(Guid ProjectId) : IQuery<IReadOnlyList<TaskForProjectViewResponse>>;
}
