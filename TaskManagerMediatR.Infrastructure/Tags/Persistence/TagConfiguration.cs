using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.Tags.Persistence
{
    public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable(TableNames.Tags);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();


            builder.Property(x => x.Name)
                .HasMaxLength(Tag.NAME_MAX_LENGTH)
                .IsRequired();

            builder.ComplexProperty(t => t.Color, s =>
            {
                s.IsRequired();
                s.Property(t => t.Code)
                .HasColumnName(nameof(Tag.Color));
            });

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
