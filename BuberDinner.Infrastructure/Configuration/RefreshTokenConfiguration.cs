
using BuberDinner.Domain.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuberDinner.Infrastructure.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Id).IsRequired();
            builder.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(256);

            builder.Property(rt => rt.UserId)
                    .IsRequired();

            builder.HasIndex(rt => rt.Id);
            builder.HasIndex(u => u.Token).IsUnique();
        }
    }
}
