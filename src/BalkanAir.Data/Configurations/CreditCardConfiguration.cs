namespace BalkanAir.Data.Configurations;

using BalkanAir.Common;
using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Number).IsRequired();
        builder.Property(c => c.NameOnCard).IsRequired();
        builder.Property(c => c.CvvNumber)
            .IsRequired()
            .HasMaxLength(ValidationConstants.CvvLength);

    }
}
