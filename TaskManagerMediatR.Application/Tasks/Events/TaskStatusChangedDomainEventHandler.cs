using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.DomainEvents;
using TaskManagerMediatR.Infrastructure.Services;

namespace TaskManagerMediatR.Application.Tasks.Events
{
    public sealed class TaskStatusChangedDomainEventHandler : IDomainEventHandler<TaskStatusChangedDomainEvent>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEmailService _emailService;

        public TaskStatusChangedDomainEventHandler(
            ITaskRepository taskRepository,
            IEmailService emailService)
        {
            _taskRepository = taskRepository;
            _emailService = emailService;
        }

        public async Task Handle(TaskStatusChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(notification.TaskId, cancellationToken);

            if (task is null)
                return;

            await _emailService.SendTaskStatusChangedEmail(task, cancellationToken);
        }
    }
}
