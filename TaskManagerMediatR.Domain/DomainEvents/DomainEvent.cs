using TaskManagerMediatR.Domain.Primitives;

namespace TaskManagerMediatR.Domain.DomainEvents
{
    public abstract record DomainEvent(Guid Id) : IDomainEvent;
}
