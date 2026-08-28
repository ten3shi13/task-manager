using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.RemoveProjectMember
{
    public sealed class RemoveProjectMemberCommandHandler : ICommandHandler<RemoveProjectMemberCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCacheInvalidator _projectCacheInvalidator;

        public RemoveProjectMemberCommandHandler(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            IProjectCacheInvalidator projectCacheInvalidator)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _projectCacheInvalidator = projectCacheInvalidator;
        }
        public async Task<Result> Handle(RemoveProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId, cancellationToken);
            if (project is null)
                return Result.Failure(DomainErrors.Project.NotFound);

            if (!project.IsOwner(request.RemovedById))
                return Result.Failure(DomainErrors.Project.UserIsNotMember);

            var user = await _userRepository.GetById(request.UserId, cancellationToken);
            if (user is null)
                return Result.Failure(DomainErrors.User.NotFound(request.UserId));

            var affectedUserIds = project.Members.Select(m => m.UserId).Append(request.UserId).ToList();

            var removeMemberResult = project.RemoveMember(request.UserId);
            if (removeMemberResult.IsFailure)
                return Result.Failure(removeMemberResult.Errors);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _projectCacheInvalidator.InvalidateProjectWithProjects(affectedUserIds, project.Id, cancellationToken);


            return Result.Success();
        }
    }
}
