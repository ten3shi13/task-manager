using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.Delete
{
    public sealed class DeleteProjectCommandHandler : ICommandHandler<DeleteProjectCommand, Guid>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCacheInvalidator _projectCacheInvalidator;

        public DeleteProjectCommandHandler(IProjectRepository projectRepository, IProjectCacheInvalidator projectCacheInvalidator)
        {
            _projectRepository = projectRepository;
            _projectCacheInvalidator = projectCacheInvalidator;
        }
        public async Task<Result<Guid>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.Id, cancellationToken);

            if (project is null)
                return Result.Failure<Guid>(DomainErrors.Project.NotFound);

            var affectedUserIds = project.Members.Select(m => m.UserId).ToList();

            var deletedRows = await _projectRepository.Delete(request.Id, cancellationToken);

            if (deletedRows == 0)
                return Result.Failure<Guid>(DomainErrors.Project.NotFound);

            await _projectCacheInvalidator.InvalidateProjectWithProjects(affectedUserIds, project.Id, cancellationToken);

            return Result.Success(request.Id);

        }
    }
}
