using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Commands.DeleteComment
{
    public sealed class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
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

            return Result.Success();
        }
    }
}
