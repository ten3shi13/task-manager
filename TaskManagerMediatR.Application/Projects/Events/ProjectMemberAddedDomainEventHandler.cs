using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.DomainEvents;
using TaskManagerMediatR.Infrastructure.Services;

namespace TaskManagerMediatR.Application.Projects.Events
{
    public sealed class ProjectMemberAddedDomainEventHandler : IDomainEventHandler<ProjectMemberAddedDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectMemberAddedDomainEventHandler(
            IEmailService emailService,
            IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }
        public async Task Handle(ProjectMemberAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(notification.UserId, cancellationToken);

            if (user is null)
                return;

            var project = await _projectRepository.GetById(notification.ProjectId, cancellationToken);

            if (project is null)
                return;

            await _emailService.SendProjectMemberAddedEmail(user, project, cancellationToken);
        }
    }
}
