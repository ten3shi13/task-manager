using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Commands.UnassignUserFromTask
{
    public sealed class UnassignUserFromTaskCommandHandler : ICommandHandler<UnassignUserFromTaskCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskCacheInvalidator _taskCacheInvalidator;

        public UnassignUserFromTaskCommandHandler(
            IUnitOfWork unitOfWork,
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            ITaskCacheInvalidator taskCacheInvalidator)
        {
            _unitOfWork = unitOfWork;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _taskCacheInvalidator = taskCacheInvalidator;
        }
        public async Task<Result> Handle(UnassignUserFromTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.TaskId, cancellationToken);
            if (task is null)
                return Result.Failure(DomainErrors.Task.NotFound);

            var project = await _projectRepository.GetById(task.ProjectId, cancellationToken);
            if (project is null || !project.IsMember(request.UnassignedById))
                return Result.Failure(DomainErrors.Project.UserIsNotMember);

            var result = task.UnassignUser(request.UserId);
            if (result.IsFailure)
                return result;

            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _taskCacheInvalidator.InvalidateTaskWithProjectTasks(task.ProjectId, task.Id, cancellationToken);

            return Result.Success();
        }
    }
}
