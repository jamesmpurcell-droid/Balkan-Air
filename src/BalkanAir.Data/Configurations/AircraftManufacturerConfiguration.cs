namespace BalkanAir.Data.Configurations;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class AircraftManufacturerConfiguration : IEntityTypeConfiguration<AircraftManufacturer>
{
    public void Configure(EntityTypeBuilder<AircraftManufacturer> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(20);
    }
}
