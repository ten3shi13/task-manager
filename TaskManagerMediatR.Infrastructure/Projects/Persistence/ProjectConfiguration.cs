using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.Projects.Persistence
{
    public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable(TableNames.Projects);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.Name)
                .HasMaxLength(Project.NAME_MAX_LENGTH)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(Project.DESCRIPTION_MAX_LENGTH);

            builder.Property(p => p.OwnerId)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Navigation(p => p.Members)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(p => p.Members, memberBuilder =>
            {
                memberBuilder.ToTable(TableNames.ProjectMembers);

                memberBuilder.WithOwner()
                    .HasForeignKey("ProjectId");  

                memberBuilder.HasKey("ProjectId", "UserId"); 

                memberBuilder.Property(m => m.UserId)
                    .IsRequired();

                memberBuilder.Property(m => m.ProjectRole)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                memberBuilder.Property(m => m.JoinedAt)
                    .HasColumnType("timestamptz")
                    .IsRequired();

                memberBuilder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OwnerId);
        }
    }
}
