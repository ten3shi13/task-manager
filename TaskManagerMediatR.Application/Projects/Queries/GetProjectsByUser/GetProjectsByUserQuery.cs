using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Contracts.Projects;

namespace TaskManagerMediatR.Application.Projects.Queries.GetProjectsByUser
{
    public sealed record GetProjectsByUserQuery(
        Guid OwnerId,
        int Page = PageNormalization.DEFAULT_PAGE,
        int PageSize = PageNormalization.DEFAULT_PAGE_SIZE,
        string? Search = null,
        Guid? MemberId = null,
        string SortBy = "createdAt",
        string SortOrder = "desc") : IQuery<PagedList<ProjectResponse>>;
}
