using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Contracts.Projects;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Queries.Get
{
    public sealed class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, IReadOnlyList<ProjectResponse>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectsQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result<IReadOnlyList<ProjectResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.Get(cancellationToken);

            if (projects is null)
                return Result.Failure<IReadOnlyList<ProjectResponse>>(DomainErrors.Project.NotFound);

            var result = projects.Select(p => new ProjectResponse(
                                p.Id,
                                p.Name,
                                p.Description,
                                p.CreatedAt,
                                p.OwnerId,
                                p.Members.Select(m => new ProjectMemberResponse(
                                    m.Id,
                                    m.ProjectRole.ToString(),
                                    m.JoinedAt)).ToList())).ToList().AsReadOnly();


            return result;

        }
    }
}
