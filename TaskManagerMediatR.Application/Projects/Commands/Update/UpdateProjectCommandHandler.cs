using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.Update
{
    public sealed class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand, Guid>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectCommandHandler(
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success(request.ProjectId);
        }
    }
}
