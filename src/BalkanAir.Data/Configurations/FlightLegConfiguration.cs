namespace BalkanAir.Data.Configurations;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class FlightLegConfiguration : IEntityTypeConfiguration<FlightLeg>
{
    public void Configure(EntityTypeBuilder<FlightLeg> builder)
    {
        builder.HasKey(fl => fl.Id);

        builder.HasOne(fl => fl.Flight)
            .WithMany(f => f.FlightLegs)
            .HasForeignKey(fl => fl.FlightId);

        builder.HasOne(fl => fl.Route)
            .WithMany(r => r.FlightLegs)
            .HasForeignKey(fl => fl.RouteId);
    }
}
