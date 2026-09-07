using Microsoft.EntityFrameworkCore;
using TaskManagerMediatR.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.RefreshTokens.Persistence
{
    public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(TableNames.RefreshTokens);

            builder.HasKey(rt => rt.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(rt => rt.TokenHash)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(rt => rt.ReplacedByTokenHash)
                .HasMaxLength(64);

            builder.HasIndex(rt => rt.TokenHash)
                .IsUnique();

            builder.HasIndex(rt => rt.TokenFamilyId);

            builder.HasIndex(rt => rt.UserId);
        }
    }
}
