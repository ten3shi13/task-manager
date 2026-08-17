using TaskManagerMediatR.Domain.Models;

using TTasks = System.Threading.Tasks;

namespace TaskManagerMediatR.Infrastructure.Services
{
    public interface IEmailService
    {
        public TTasks.Task SendProjectMemberAddedEmail(User user, Project project, CancellationToken cancellationToken = default);
        public TTasks.Task SendTaskAssignedEmail(Domain.Models.Task task, User user, CancellationToken cancellationToken = default);
        public TTasks.Task SendTaskUnassignedEmail(Domain.Models.Task task, User user, CancellationToken cancellationToken = default);
        public TTasks.Task SendTaskCompletedEmail(Domain.Models.Task task, CancellationToken cancellationToken = default);
        public TTasks.Task SendTaskStatusChangedEmail(Domain.Models.Task task, CancellationToken cancellationToken = default);
    }
}