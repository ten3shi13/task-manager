using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Contracts.Projects;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Queries.Get
{
    public sealed class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, PagedList<ProjectResponse>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectsQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result<PagedList<ProjectResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var query = _projectRepository.GetFiltered(
                request.Search,
                request.OwnerId,
                request.MemberId,
                request.SortBy,
                request.SortOrder);

            var projected = query.Select(p => new ProjectResponse(
                p.Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.OwnerId,
                p.Members.Select(m => new ProjectMemberResponse(
                    m.Id,
                    m.ProjectRole.ToString(),
                    m.JoinedAt)).ToList()));

            var page = await PagedList<ProjectResponse>.CreateAsync(
                projected,
                request.Page,
                request.PageSize,
                cancellationToken);

            return Result.Success(page);

        }
    }
}
