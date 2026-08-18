using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Outbox;

namespace TaskManagerMediatR.Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public sealed class ProcessOutboxMessagesJob : IJob
    {
        private const int BATCH_SIZE = 30;
        private const int MAX_RETRIES = 3;

        private readonly TaskManagerMediatRDbContext _context;
        private readonly IPublisher _publisher;

        public ProcessOutboxMessagesJob(TaskManagerMediatRDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _context.Set<OutboxMessage>()
                .Where(om => om.ProcessedOnUtc == null && om.RetryCount < MAX_RETRIES)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(BATCH_SIZE)
                .ToListAsync(context.CancellationToken);

            foreach (OutboxMessage outboxMessage in messages)
            {
                IDomainEvent? domainEvent = JsonConvert
                    .DeserializeObject<IDomainEvent>(
                        outboxMessage.Content,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });

                if (domainEvent is null)
                {
                    outboxMessage.Error = "Deserialization returned null";
                    outboxMessage.RetryCount++;
                    continue;
                }

                await _publisher.Publish(domainEvent, context.CancellationToken);

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                outboxMessage.Error = null;
            }

            await _context.SaveChangesAsync();
        }
    }
}
