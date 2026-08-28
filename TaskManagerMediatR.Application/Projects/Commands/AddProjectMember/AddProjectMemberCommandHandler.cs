using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.AddProjectMember
{
    public sealed class AddProjectMemberCommandHandler : ICommandHandler<AddProjectMemberCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCacheInvalidator _projectCacheInvalidator;

        public AddProjectMemberCommandHandler(
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

        public async Task<Result> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId, cancellationToken);
            if (project is null)
                return Result.Failure(DomainErrors.Project.NotFound);

            if (!project.IsOwner(request.AddedById))
                return Result.Failure(DomainErrors.Project.UserIsNotMember);

            var user = await _userRepository.GetById(request.UserId, cancellationToken);
            if (user is null)
                return Result.Failure(DomainErrors.User.NotFound(request.UserId));

            var addMemberResult = project.AddMember(request.UserId);
            if (addMemberResult.IsFailure)
                return Result.Failure(addMemberResult.Errors);

            var affectedUserIds = project.Members.Select(m => m.UserId).Append(request.UserId).ToList();

            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _projectCacheInvalidator.InvalidateProjectWithProjects(affectedUserIds, project.Id, cancellationToken);

            return Result.Success();
        }
    }
}
