namespace TaskManagerMediatR.Domain.DomainEvents
{
    public sealed record TaskStatusChangedDomainEvent(Guid Id, Guid TaskId, Guid UserId) : DomainEvent(Id);

}
