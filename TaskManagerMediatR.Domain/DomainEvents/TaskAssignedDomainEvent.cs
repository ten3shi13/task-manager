namespace TaskManagerMediatR.Domain.DomainEvents
{
    public sealed record TaskAssignedDomainEvent(Guid Id, Guid TaskId, Guid UserId) : DomainEvent(Id);
   
}
