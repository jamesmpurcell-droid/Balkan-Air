namespace BalkanAir.Data.Configurations;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email).IsRequired();

        builder.OwnsOne(u => u.UserSettings, settings =>
        {
            settings.Property(s => s.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(50);

            settings.Property(s => s.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(50);

            settings.Property(s => s.DateOfBirth)
                .HasColumnName("DateOfBirth");

            settings.Property(s => s.Gender)
                .HasColumnName("Gender");

            settings.Property(s => s.IdentityDocumentNumber)
                .HasColumnName("IdentityDocumentNumber")
                .HasMaxLength(20);

            settings.Property(s => s.Nationality)
                .HasColumnName("Nationality");

            settings.Property(s => s.FullAddress)
                .HasColumnName("FullAddress");

            settings.Property(s => s.ProfilePicture)
                .HasColumnName("ProfilePicture");

            settings.Property(s => s.ReceiveEmailWhenNewNews)
                .HasColumnName("ReceiveEmailWhenNewNews");

            settings.Property(s => s.ReceiveEmailWhenNewFlight)
                .HasColumnName("ReceiveEmailWhenNewFlight");

            settings.Property(s => s.ReceiveNotificationWhenNewNews)
                .HasColumnName("ReceiveNotificationWhenNewNews");

            settings.Property(s => s.ReceiveNotificationWhenNewFlight)
                .HasColumnName("ReceiveNotificationWhenNewFlight");

            settings.Property(s => s.LastLogin)
                .HasColumnName("LastLogin")
                .HasColumnType("datetime2");

            settings.Property(s => s.LastLogout)
                .HasColumnName("LastLogout")
                .HasColumnType("datetime2");

            settings.Ignore(s => s.Bookings);
        });
    }
}
