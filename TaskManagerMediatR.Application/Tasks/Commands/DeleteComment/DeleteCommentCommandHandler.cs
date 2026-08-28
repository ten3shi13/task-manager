using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Commands.DeleteComment
{
    public sealed class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskCacheInvalidator _taskCacheInvalidator;

        public DeleteCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ITaskRepository taskRepository,
            ITaskCacheInvalidator taskCacheInvalidator)
        {
            _unitOfWork = unitOfWork;
            _taskRepository = taskRepository;
            _taskCacheInvalidator = taskCacheInvalidator;
        }

        public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.TaskId, cancellationToken);
            if (task is null)
                return Result.Failure(DomainErrors.Task.NotFound);

            var result = task.DeleteComment(request.CommentId, request.DeletedById);
            if (result.IsFailure)
                return result;

            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _taskCacheInvalidator.InvalidateTaskWithProjectTasks(task.ProjectId, task.Id, cancellationToken);

            return Result.Success();
        }
    }
}
