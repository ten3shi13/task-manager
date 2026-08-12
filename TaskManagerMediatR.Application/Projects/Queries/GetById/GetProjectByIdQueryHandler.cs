using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Contracts.Projects;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Queries.GetById
{
    public sealed class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
    {
        private readonly IProjectRepository _projectRepository;
        public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.Id, cancellationToken);

            if (project == null)
                return Result.Failure<ProjectResponse>(DomainErrors.Project.NotFound);

            var response = new ProjectResponse(
                                    project.Id,
                                    project.Name,
                                    project.Description,
                                    project.CreatedAt,
                                    project.OwnerId,
                                    project.Members.Select(m => new ProjectMemberResponse(
                                        m.Id,
                                        m.ProjectRole.ToString(),
                                        m.JoinedAt)).ToList());

            return response;

        }
    }
}
