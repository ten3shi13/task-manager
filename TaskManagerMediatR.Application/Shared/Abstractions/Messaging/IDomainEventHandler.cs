using MediatR;
using TaskManagerMediatR.Domain.Primitives;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Messaging
{
    public interface IDomainEventHandler<Tevent> : INotificationHandler<Tevent>
        where Tevent : IDomainEvent;
}
