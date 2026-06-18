namespace BalkanAir.Data.Configurations;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class LegInstanceConfiguration : IEntityTypeConfiguration<LegInstance>
{
    public void Configure(EntityTypeBuilder<LegInstance> builder)
    {
        builder.HasKey(li => li.Id);

        builder.Property(li => li.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(li => li.FlightLeg)
            .WithMany(fl => fl.LegInstances)
            .HasForeignKey(li => li.FlightLegId);

        builder.HasOne(li => li.FlightStatus)
            .WithMany(fs => fs.LegInstances)
            .HasForeignKey(li => li.FlightStatusId);

        builder.HasOne(li => li.Aircraft)
            .WithMany(a => a.LegInstances)
            .HasForeignKey(li => li.AircraftId);

        builder.Ignore(li => li.Duration);
    }
}
