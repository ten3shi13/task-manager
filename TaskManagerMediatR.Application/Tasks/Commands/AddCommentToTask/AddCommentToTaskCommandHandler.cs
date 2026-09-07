using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Commands.AddCommentToTask
{
    internal class AddCommentToTaskCommandHandler : ICommandHandler<AddCommentToTaskCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskCacheInvalidator _taskCacheInvalidator;

        public AddCommentToTaskCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            ITaskCacheInvalidator taskCacheInvalidator)
        {
            _unitOfWork = unitOfWork;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _taskCacheInvalidator = taskCacheInvalidator;
            _currentUser = currentUser;
        }
        public async Task<Result<Guid>> Handle(AddCommentToTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.Id, cancellationToken);
            if (task is null)
                return Result.Failure<Guid>(DomainErrors.Task.NotFound);

            var project = await _projectRepository.GetById(task.ProjectId, cancellationToken);
            if (project is null)
                return Result.Failure<Guid>(DomainErrors.Project.NotFound);

            if (!project.IsMember(request.AuthorId) || !_currentUser.IsInRole(Roles.Admin))
                return Result.Failure<Guid>(DomainErrors.Project.UserIsNotMember);

            var result = task.AddComment(request.AuthorId, request.Text);
            if (result.IsFailure)
                return Result.Failure<Guid>(result.Errors);
            
            await _unitOfWork.CommitChangesAsync(cancellationToken);
            await _taskCacheInvalidator.InvalidateTaskWithProjectTasks(task.ProjectId, task.Id, cancellationToken);

            return result.Value.Id;
        }
    }
}
