using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.DomainEvents;
using TaskManagerMediatR.Infrastructure.Services;

namespace TaskManagerMediatR.Application.Tasks.Events
{
    public sealed class TaskAssignedDomainEventHandler : IDomainEventHandler<TaskAssignedDomainEvent>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public TaskAssignedDomainEventHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IEmailService emailService)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(TaskAssignedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(notification.UserId, cancellationToken);

            if (user is null)
                return;

            var task = await _taskRepository.GetById(notification.TaskId, cancellationToken);

            if (task is null)
                return;

            await _emailService.SendTaskAssignedEmail(task, user, cancellationToken);
        }
    }
}
