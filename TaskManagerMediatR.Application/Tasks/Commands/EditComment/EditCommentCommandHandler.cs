using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Commands.EditComment
{
    public sealed class EditCommentCommandHandler : ICommandHandler<EditCommentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskCacheInvalidator _taskCacheInvalidator;

        public EditCommentCommandHandler(
            IUnitOfWork unitOfWork,
            ITaskRepository taskRepository,
            ITaskCacheInvalidator taskCacheInvalidator)
        {
            _unitOfWork = unitOfWork;
            _taskRepository = taskRepository;
            _taskCacheInvalidator = taskCacheInvalidator;
        }
        public async Task<Result> Handle(EditCommentCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.Id, cancellationToken);
            if (task is null)
                return Result.Failure(DomainErrors.Task.NotFound);

            var result = task.EditComment(request.CommentId, request.EditorId, request.Text);
            if (result.IsFailure)
                return result;

            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _taskCacheInvalidator.InvalidateTaskWithProjectTasks(task.ProjectId, task.Id, cancellationToken);

            return Result.Success();
        }
    }
}
