using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContosoAutoTech.Data.Entities.Configurations;

public class CarReferencePriceConfiguration : IEntityTypeConfiguration<CarReferencePrice>
{
    public void Configure(EntityTypeBuilder<CarReferencePrice> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Model)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(x => x.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();
    }
}

