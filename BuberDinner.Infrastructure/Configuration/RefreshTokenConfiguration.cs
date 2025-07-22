using BuberDinner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuberDinner.Infrastructure.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.Property(rt => rt.Id).IsRequired();

            builder.Property(rt => rt.Token).IsRequired().HasMaxLength(256);

            builder
                .Metadata.FindNavigation(nameof(User.RefreshTokens))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(rt => rt.UserId);
        }
    }
}
