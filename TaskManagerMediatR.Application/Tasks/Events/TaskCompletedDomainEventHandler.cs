using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.DomainEvents;
using TaskManagerMediatR.Infrastructure.Services;

namespace TaskManagerMediatR.Application.Tasks.Events
{
    public sealed class TaskCompletedDomainEventHandler : IDomainEventHandler<TaskCompletedDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ITaskRepository _taskRepository;

        public TaskCompletedDomainEventHandler(
            IEmailService emailService,
            ITaskRepository taskRepository)
        {
            _emailService = emailService;
            _taskRepository = taskRepository;
        }

        public async Task Handle(TaskCompletedDomainEvent notification, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(notification.TaskId, cancellationToken);

            if (task is null)
                return;

            await _emailService.SendTaskStatusChangedEmail(task, cancellationToken);
        }
    }
}
