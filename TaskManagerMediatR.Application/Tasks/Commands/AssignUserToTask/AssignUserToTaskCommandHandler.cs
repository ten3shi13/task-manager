using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Commands.AssignUserToTask
{
    public sealed class AssignUserToTaskCommandHandler : ICommandHandler<AssignUserToTaskCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskCacheInvalidator _taskCacheInvalidator;

        public AssignUserToTaskCommandHandler(
            IUnitOfWork unitOfWork,
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            ITaskCacheInvalidator taskCacheInvalidator)
        {
            _unitOfWork = unitOfWork;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _taskCacheInvalidator = taskCacheInvalidator;
        }
        public async Task<Result> Handle(AssignUserToTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.TaskId, cancellationToken);
            if (task is null)
                return Result.Failure(DomainErrors.Task.NotFound);

            var project = await _projectRepository.GetById(task.ProjectId, cancellationToken);
            if (project is null)
                return Result.Failure(DomainErrors.Project.NotFound);

            if (!project.IsMember(request.AssignedById))
                return Result.Failure(DomainErrors.Project.UserIsNotMember);

            if (!project.IsMember(request.UserId))
                return Result.Failure(DomainErrors.Project.UserIsNotMember);

            var user = await _userRepository.GetById(request.UserId, cancellationToken);
            if (user is null)
                return Result.Failure(DomainErrors.User.NotFound(request.UserId));

            var result = task.AssignUser(request.UserId, request.AssignedById);
            if (result.IsFailure)
                return result;

            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _taskCacheInvalidator.InvalidateTaskWithProjectTasks(task.ProjectId, task.Id, cancellationToken);

            return Result.Success();
        }
    }
}
