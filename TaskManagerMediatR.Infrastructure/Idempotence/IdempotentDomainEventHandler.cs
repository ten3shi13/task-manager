using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Outbox;

namespace TaskManagerMediatR.Infrastructure.Idempotence
{
    internal sealed class IdempotentDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        private readonly INotificationHandler<TDomainEvent> _decorated;
        private readonly TaskManagerMediatRDbContext _context;

        public IdempotentDomainEventHandler(
            INotificationHandler<TDomainEvent> decorated,
            TaskManagerMediatRDbContext context)
        {
            _decorated = decorated;
            _context = context;
        }

        public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
        {
            var consumerName = _decorated.GetType().AssemblyQualifiedName ?? _decorated.GetType().Name;

            var alreadyProcessed = await _context.Set<OutboxMessageConsumer>()
                .AnyAsync(omc => omc.Id == notification.Id && omc.Name == consumerName, cancellationToken);

            if (alreadyProcessed)
                return;

            await _decorated.Handle(notification, cancellationToken);

            _context.Set<OutboxMessageConsumer>().Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumerName
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
