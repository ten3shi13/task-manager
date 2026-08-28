namespace TaskManagerMediatR.Domain.DomainEvents
{
    public sealed record ProjectMemberAddedDomainEvent(
        Guid Id,
        Guid ProjectId,
        Guid UserId) : DomainEvent(Id);
}
