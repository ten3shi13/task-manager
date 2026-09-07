using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Commands.Delete
{
    public sealed class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskCacheInvalidator _taskCacheInvalidator;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository, ITaskCacheInvalidator taskCacheInvalidator)
        {
            _taskRepository = taskRepository;
            _taskCacheInvalidator = taskCacheInvalidator;
        }
        public async Task<Result<Guid>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.Id, cancellationToken);

            if (task is null)
                return Result.Failure<Guid>(DomainErrors.Task.NotFound);

            var deletedRows = await _taskRepository.Delete(request.Id, cancellationToken);

            if (deletedRows == 0)
                return Result.Failure<Guid>(DomainErrors.Task.NotFound);

            await _taskCacheInvalidator.InvalidateTaskWithProjectTasks(task.ProjectId, task.Id, cancellationToken);

            return Result.Success(request.Id);

        }
    }
}
