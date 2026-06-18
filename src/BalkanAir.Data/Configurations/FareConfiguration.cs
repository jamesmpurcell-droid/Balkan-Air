namespace BalkanAir.Data.Configurations;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class FareConfiguration : IEntityTypeConfiguration<Fare>
{
    public void Configure(EntityTypeBuilder<Fare> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(f => f.Route)
            .WithMany(r => r.Fares)
            .HasForeignKey(f => f.RouteId);
    }
}
