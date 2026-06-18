namespace BalkanAir.Data.Configurations;

using BalkanAir.Common;
using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.ConfirmationCode)
            .IsRequired()
            .HasMaxLength(ValidationConstants.ConfirmationCodeLength);

        builder.HasIndex(b => b.ConfirmationCode).IsUnique();

        builder.Property(b => b.SeatNumber)
            .IsRequired()
            .HasMaxLength(1);

        builder.Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId);

        builder.HasOne(b => b.LegInstance)
            .WithMany(l => l.Bookings)
            .HasForeignKey(b => b.LegInstanceId);
    }
}
