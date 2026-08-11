using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Domain.ValueObjects;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.Users.Persistence
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(TableNames.Users);

            builder.HasKey(u => u.Id);
            
            builder.Property(u =>u.Id)
                .ValueGeneratedNever();

            builder.ComplexProperty(u => u.FirstName, e => {

                e.IsRequired();
                e.Property(e => e.Value)
                    .HasMaxLength(FirstName.FIRST_NAME_MAX_LENGTH)
                    .HasColumnName(nameof(User.FirstName));

            });

            builder.ComplexProperty(u => u.Email, e => {

                e.IsRequired();
                e.Property(e => e.Value)
                    .HasMaxLength(Email.EMAIL_MAX_LENGTH)
                    .HasColumnName(nameof(User.Email));

            });

            builder.Property(u => u.PasswordHash)
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnType("timestamptz")
                .IsRequired();

            //builder.HasIndex(nameof(User.Email))
            //    .IsUnique();
        }
    }
}
