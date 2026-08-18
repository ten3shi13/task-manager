using TaskManagerMediatR.Domain.Models;

using TTasks = System.Threading.Tasks;

namespace TaskManagerMediatR.Infrastructure.Services
{
    internal sealed class EmailService : IEmailService
    {
        public TTasks.Task SendProjectMemberAddedEmail(User user, Project project, CancellationToken cancellationToken = default) =>
            TTasks.Task.CompletedTask;

        public TTasks.Task SendTaskAssignedEmail(Domain.Models.Task task, User user, CancellationToken cancellationToken = default) =>
            TTasks.Task.CompletedTask;

        public TTasks.Task SendTaskCompletedEmail(Domain.Models.Task task, CancellationToken cancellationToken = default) =>
            TTasks.Task.CompletedTask;

        public TTasks.Task SendTaskStatusChangedEmail(Domain.Models.Task task, CancellationToken cancellationToken = default) =>
            TTasks.Task.CompletedTask;

        public TTasks.Task SendTaskUnassignedEmail(Domain.Models.Task task, User user, CancellationToken cancellationToken = default) =>
            TTasks.Task.CompletedTask;
    }
}
