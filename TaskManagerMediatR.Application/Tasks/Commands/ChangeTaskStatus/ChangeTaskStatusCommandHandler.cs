using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Application.Tasks.Commands.ChangeTaskStatus
{
    public sealed class ChangeTaskStatusCommandHandler : ICommandHandler<ChangeTaskStatusCommand>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeTaskStatusCommandHandler(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.Id, cancellationToken);
            if (task is null)
                return Result.Failure(DomainErrors.Task.NotFound);

            var project = await _projectRepository.GetById(task.Id, cancellationToken);
            if (project is null || !project.IsMember(request.ChangedById))
                return Result.Failure(DomainErrors.Project.UserIsNotMember);

            var statusResult = Status.FromValue(request.Status);
            if (statusResult.IsFailure)
                return Result.Failure(statusResult.Error);

            var result = task.ChangeStatus(statusResult.Value);
            if (result.IsFailure)
                return result;

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
