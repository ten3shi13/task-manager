using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Commands.AddCommentToTask
{
    internal class AddCommentToTaskCommandHandler : ICommandHandler<AddCommentToTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddCommentToTaskCommandHandler(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(AddCommentToTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.Id, cancellationToken);
            if (task is null)
                return Result.Failure<Guid>(DomainErrors.Task.NotFound);

            var project = await _projectRepository.GetById(task.ProjectId, cancellationToken);
            if (project is null || !project.IsMember(request.AuthorId))
                return Result.Failure<Guid>(DomainErrors.Project.UserIsNotMember);

            var result = task.AddComment(request.AuthorId, request.Text);
            if (result.IsFailure)
                return Result.Failure<Guid>(result.Errors);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return result.Value.Id;
        }
    }
}
