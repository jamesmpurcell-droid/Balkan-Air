namespace BalkanAir.Data.Configurations;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class TravelClassConfiguration : IEntityTypeConfiguration<TravelClass>
{
    public void Configure(EntityTypeBuilder<TravelClass> builder)
    {
        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Meal)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(tc => tc.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(tc => tc.Aircraft)
            .WithMany(a => a.TravelClasses)
            .HasForeignKey(tc => tc.AircraftId);
    }
}
