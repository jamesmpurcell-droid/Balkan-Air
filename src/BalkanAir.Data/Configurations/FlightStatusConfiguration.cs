namespace BalkanAir.Data.Configurations;

using BalkanAir.Common;
using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class FlightStatusConfiguration : IEntityTypeConfiguration<FlightStatus>
{
    public void Configure(EntityTypeBuilder<FlightStatus> builder)
    {
        builder.HasKey(fs => fs.Id);

        builder.Property(fs => fs.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.FlightStatusNameMaxLength);

        builder.HasIndex(fs => fs.Name).IsUnique();
    }
}
