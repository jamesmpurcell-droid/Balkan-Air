namespace BalkanAir.Data.Configurations;

using BalkanAir.Common;
using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.CountryNameMaxLength);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.Abbreviation)
            .IsRequired()
            .HasMaxLength(ValidationConstants.CountryAbbreviationMaxLength);

        builder.HasIndex(c => c.Abbreviation).IsUnique();
    }
}
