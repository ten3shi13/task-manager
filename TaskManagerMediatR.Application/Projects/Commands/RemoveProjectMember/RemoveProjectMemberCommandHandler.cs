using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.RemoveProjectMember
{
    public sealed class RemoveProjectMemberCommandHandler : ICommandHandler<RemoveProjectMemberCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProjectMemberCommandHandler(
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
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

            var removeMemberResult = project.RemoveMember(request.UserId);
            if (removeMemberResult.IsFailure)
                return Result.Failure(removeMemberResult.Error);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
