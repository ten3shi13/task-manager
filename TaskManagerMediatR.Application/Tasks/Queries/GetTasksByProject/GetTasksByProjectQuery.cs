using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Contracts.Tasks;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetTasksByProject
{
    public sealed record GetTasksByProjectQuery(
        Guid ProjectId,
        int Page = PageNormalization.DEFAULT_PAGE,
        int PageSize = PageNormalization.DEFAULT_PAGE_SIZE,
        string? Status = null,
        string? Priority = null,
        Guid? AssigneeId = null,
        string? Search = null,
        string? SortBy = "createdAt",
        string? SortOrder = "desc") : IQuery<PagedList<TaskForProjectViewResponse>>;
}
