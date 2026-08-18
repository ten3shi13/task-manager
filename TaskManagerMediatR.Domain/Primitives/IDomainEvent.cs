using MediatR;

namespace TaskManagerMediatR.Domain.Primitives
{
    public interface IDomainEvent : INotification
    {
        public Guid Id { get; init; }
    }
}
