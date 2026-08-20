using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Contracts.Projects;

namespace TaskManagerMediatR.Application.Projects.Queries.Get
{
    public sealed record GetProjectsQuery(
        int Page = 1,
        int PageSize = 20,
        string? Search = null,
        Guid? OwnerId = null,
        Guid? MemberId = null,
        string SortBy = "createdAt",
        string SortOrder = "desc") : IQuery<PagedList<ProjectResponse>>;
}
