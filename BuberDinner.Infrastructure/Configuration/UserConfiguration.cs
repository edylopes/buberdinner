using BuberDinner.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuberDinner.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.Id)
                  .IsRequired();


            builder.Property(u => u.PasswordHash)
                 .IsRequired()
                 .HasMaxLength(70);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.FirstName);
            builder.HasIndex(u => u.Id);
            builder.HasIndex(u => u.Email).IsUnique();


            builder.HasMany<RefreshToken>("_refreshTokens") // <– referência ao campo privado
                .WithOne(r => r.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation("_refreshTokens")
               .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(u => u.Roles)
                .HasConversion(UserRoleConverter.Converter)
                .HasColumnType("TEXT");

            builder.Property(u => u.RowVersion)
                 .IsRowVersion();


        }
    }
}
