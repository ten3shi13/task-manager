using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Outbox;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Interceptors
{
    public sealed class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {

            DbContext? context = eventData.Context;

            if(context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);


            var outboxMessages = context.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .SelectMany(ar =>
                {
                    var domainEvents = ar.GetDomainEvents();

                    ar.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(de => new OutboxMessage
                {
                    Id = de.Id,
                    OccurredOnUtc = DateTime.UtcNow,
                    Type = de.GetType().AssemblyQualifiedName
                       ?? de.GetType().Name,
                    Content = JsonConvert.SerializeObject(
                    de,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
                })
            .ToList();

            if (outboxMessages.Count > 0)
                context.Set<OutboxMessage>().AddRange(outboxMessages);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
