using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Outbox.Persistence
{
    public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable(TableNames.OutboxMessages);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.Error)
                .HasMaxLength(4000);

            builder.HasIndex(x => x.ProcessedOnUtc);
        }
    }
}
