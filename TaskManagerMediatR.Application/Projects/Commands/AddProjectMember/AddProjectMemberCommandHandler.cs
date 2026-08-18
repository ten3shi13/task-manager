using MediatR;
using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.AddProjectMember
{
    public sealed class AddProjectMemberCommandHandler : ICommandHandler<AddProjectMemberCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddProjectMemberCommandHandler(
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
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
                return Result.Failure(addMemberResult.Error);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
