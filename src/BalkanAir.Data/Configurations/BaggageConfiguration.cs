namespace BalkanAir.Data.Configurations;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class BaggageConfiguration : IEntityTypeConfiguration<Baggage>
{
    public void Configure(EntityTypeBuilder<Baggage> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.Booking)
            .WithMany(bk => bk.Baggage)
            .HasForeignKey(b => b.BookingId);
    }
}
