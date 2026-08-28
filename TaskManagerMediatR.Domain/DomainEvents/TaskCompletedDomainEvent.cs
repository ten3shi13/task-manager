namespace TaskManagerMediatR.Domain.DomainEvents
{
    public sealed record TaskCompletedDomainEvent(
        Guid Id,
        Guid TaskId) : DomainEvent(Id);

}
