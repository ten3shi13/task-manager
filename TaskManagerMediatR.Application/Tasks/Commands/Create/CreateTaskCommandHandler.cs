using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Application.Tasks.Commands.Create
{
    public sealed class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, Guid>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateTaskCommandHandler(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId, cancellationToken);
            if (project is null)
                return Result.Failure<Guid>(DomainErrors.Project.NotFound);

            if (!project.IsMember(request.CreatedById))
                return Result.Failure<Guid>(DomainErrors.Project.UserIsNotMember);

            var priorityResult = Priority.FromValue(request.Priority);
            if (priorityResult.IsFailure)
                return Result.Failure<Guid>(priorityResult.Errors);

            var taskResult = Domain.Models.Task.Create(
                Guid.NewGuid(),
                request.ProjectId,
                request.Title,
                request.Description,
                Status.Todo,
                priorityResult.Value,
                request.CreatedById,
                request.DueDate);

            if (taskResult.IsFailure)
                return Result.Failure<Guid>(taskResult.Errors);

            await _taskRepository.Add(taskResult.Value, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return taskResult.Value.Id;

        }
    }
}
