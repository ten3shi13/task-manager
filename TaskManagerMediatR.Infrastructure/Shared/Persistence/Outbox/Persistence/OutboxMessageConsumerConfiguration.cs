using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Outbox.Persistence
{
    public sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
        {
            builder.ToTable(TableNames.OutboxMessageConsumers);

            builder.HasKey(omc => new { omc.Id, omc.Name });

            builder.Property(omc => omc.Name)
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}
