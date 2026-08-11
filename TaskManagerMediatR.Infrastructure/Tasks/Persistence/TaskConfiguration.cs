using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerMediatR.Domain.ValueObjects;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.Tasks.Persistence
{
    public sealed class TaskConfiguration : IEntityTypeConfiguration<Domain.Models.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Task> builder)
        {
            builder.ToTable(TableNames.Tasks);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.ProjectId)
                .IsRequired();

            builder.Property(t => t.Title)
                .HasMaxLength(Domain.Models.Task.TITLE_MAX_LENGTH)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(Domain.Models.Task.DESCRIPTION_MAX_LENGTH)
                .IsRequired();

            builder.ComplexProperty(t => t.Status, s =>
            {
                s.IsRequired();
                s.Property(t => t.Value)
                .HasColumnName(nameof(Domain.Models.Task.Status));
            });

            builder.ComplexProperty(t => t.Priority, p =>
            {
                p.IsRequired();
                p.Property(t => t.Value)
                .HasColumnName(nameof(Domain.Models.Task.Priority));
            });

            builder.Property(t => t.DueDate)
                .HasColumnType("timestamptz");

            builder.Property(t => t.CreatedAt)
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.HasMany(t => t.Comments)
                .WithOne()
                .HasForeignKey("TaskId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Assignments)
                .WithOne()
                .HasForeignKey("TaskId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(t => t.Assignments)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(t => t.Comments)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(t => t.Tags)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(t => t.Tags)
                .WithMany();

            builder.HasIndex(t => t.ProjectId);

            //builder.HasIndex(t => new { 
            //    t.ProjectId,
            //    t.Status 
            //});

            builder.HasIndex(t => t.CreatedById);

            builder.HasIndex(t => t.DueDate);

        }
    }
}
