using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.Assigments.Persistence
{
    public class AssigmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable(TableNames.Assignments);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.AssignedBy)
                .IsRequired();

            builder.Property(x => x.AssignedAt)
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasIndex("TaskId", nameof(Assignment.UserId))
                .IsUnique();

            builder.HasIndex(x => x.UserId);
        }
    }
}
