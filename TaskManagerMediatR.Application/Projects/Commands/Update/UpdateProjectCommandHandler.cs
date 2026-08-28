using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.Update
{
    public sealed class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCacheInvalidator _projectCacheInvalidator;

        public UpdateProjectCommandHandler(
            IUnitOfWork unitOfWork,
            IProjectRepository projectRepository,
            IProjectCacheInvalidator projectCacheInvalidator)
        {
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _projectCacheInvalidator = projectCacheInvalidator;
        }

        public async Task<Result<Guid>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId, cancellationToken);
            if (project is null)
                return Result.Failure<Guid>(DomainErrors.Project.NotFound);

            if(!project.IsOwner(request.UpdatedById))
                return Result.Failure<Guid>(DomainErrors.Project.UserIsNotMember);

            var updateProjectResult = project.UpdateDetails(request.Name, request.Description);
            if (updateProjectResult.IsFailure)
                return Result.Failure<Guid>(updateProjectResult.Errors);

            var affectedUserIds = project.Members.Select(g => g.UserId).ToList();

            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _projectCacheInvalidator.InvalidateProjectWithProjects(affectedUserIds, project.Id, cancellationToken);

            return Result.Success(request.ProjectId);
        }
    }
}
