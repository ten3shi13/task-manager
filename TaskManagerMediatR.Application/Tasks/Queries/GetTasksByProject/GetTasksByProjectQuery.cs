using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Contracts.Tasks;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetTasksByProject
{
    public sealed record GetTasksByProjectQuery(
        Guid ProjectId,
        int Page = 1,
        int PageSize = 20,
        string? Status = null,
        string? Priority = null,
        Guid? AssigneeId = null,
        string? Search = null,
        string? SortBy = "createdAt",
        string? SortOrder = "desc") : IQuery<PagedList<TaskForProjectViewResponse>>;
}
