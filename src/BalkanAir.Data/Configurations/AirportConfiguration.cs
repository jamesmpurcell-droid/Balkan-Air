namespace BalkanAir.Data.Configurations;

using BalkanAir.Common;
using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.AirportNameMaxLength);

        builder.Property(a => a.Abbreviation)
            .IsRequired()
            .HasMaxLength(ValidationConstants.AirportAbbreviationMaxLength);

        builder.HasIndex(a => a.Abbreviation).IsUnique();

        builder.HasOne(a => a.Country)
            .WithMany(c => c.Airports)
            .HasForeignKey(a => a.CountryId);

        builder.HasMany(a => a.Origins)
            .WithOne(r => r.Origin)
            .HasForeignKey(r => r.OriginId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Destinations)
            .WithOne(r => r.Destination)
            .HasForeignKey(r => r.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
