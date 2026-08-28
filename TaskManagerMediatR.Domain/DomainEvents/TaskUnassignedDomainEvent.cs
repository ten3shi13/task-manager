namespace TaskManagerMediatR.Domain.DomainEvents
{
    public sealed record TaskUnassignedDomainEvent(
        Guid Id,
        Guid TaskId,
        Guid UserId) : DomainEvent(Id);

}
