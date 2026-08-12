using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Application.Tasks.Commands.Update
{
    public sealed class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskCommandHandler(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.Id, cancellationToken);
            if (task is null)
                return Result.Failure(DomainErrors.Task.NotFound);

            var project = await _projectRepository.GetById(task.Id, cancellationToken);
            if (project is null || !project.IsMember(request.UpdatedById))
                return Result.Failure(DomainErrors.Project.UserIsNotMember);

            var priorityResult = Priority.FromValue(request.Priority);
            if (priorityResult.IsFailure)
                return Result.Failure(priorityResult.Error);

            var result = task.UpdateDetails(
                request.Title,
                request.Description,
                priorityResult.Value,
                request.DueDate);

            if (result.IsFailure)
                return result;

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
