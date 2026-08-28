using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.Create
{
    public sealed class СreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCacheInvalidator _projectCacheInvalidator;

        public СreateProjectCommandHandler(
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
        public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetById(request.OwnerId, cancellationToken);
            if (owner is null)
                return Result.Failure<Guid>(DomainErrors.User.NotFound(request.OwnerId));

            var projectResult = Project.Create(Guid.NewGuid(), request.Name, request.Description, request.OwnerId);
            if (projectResult.IsFailure)
                return Result.Failure<Guid>(projectResult.Errors);

            await _projectRepository.Add(projectResult.Value, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _projectCacheInvalidator.InvalidateProjects([projectResult.Value.OwnerId], cancellationToken);

            return projectResult.Value.Id;
        }
    }
}
