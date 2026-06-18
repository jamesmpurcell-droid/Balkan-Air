namespace BalkanAir.Data.Configurations;

using BalkanAir.Common;
using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class AircraftConfiguration : IEntityTypeConfiguration<Aircraft>
{
    public void Configure(EntityTypeBuilder<Aircraft> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Model)
            .IsRequired()
            .HasMaxLength(ValidationConstants.AircraftModelMaxLength);

        builder.Property(a => a.TotalSeats)
            .IsRequired();

        builder.HasOne(a => a.AircraftManufacturer)
            .WithMany(m => m.Aircrafts)
            .HasForeignKey(a => a.AircraftManufacturerId);

        builder.Ignore(a => a.CheapestTravelClassPrice);
    }
}
