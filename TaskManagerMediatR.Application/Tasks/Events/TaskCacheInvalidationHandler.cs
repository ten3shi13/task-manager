using MediatR;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;

namespace TaskManagerMediatR.Application.Tasks.Events
{
    //internal sealed class TaskCacheInvalidationHandler : 
    //    INotificationHandler<TaskUpdatedDomainEvent>,
    //    INotificationHandler<TaskStatusChangedDomainEvent>,
    //    INotificationHandler<TaskCommentAddedDomainEvent>
    //{
    //    private readonly ITaskCacheInvalidator _invalidator;

    //    public Task Handle(TaskUpdatedDomainEvent e, CancellationToken ct)
    //        => _invalidator.InvalidateTaskAsync(e.TaskId, e.ProjectId, ct);
    //}
}
