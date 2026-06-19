namespace BalkanAir.Data.Configurations;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Number)
            .IsRequired()
            .HasMaxLength(1);

        builder.HasOne(s => s.LegInstance)
            .WithMany(li => li.Seats)
            .HasForeignKey(s => s.LegInstanceId);
    }
}
