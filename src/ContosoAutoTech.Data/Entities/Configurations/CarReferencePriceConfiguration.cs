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

        // Seed data
        builder.HasData(
            new
            {
                Id = Guid.NewGuid(),
                Model = "Toyota Corolla XEi 2.0",
                Price = 110000.00m,
                CreatedAt = new DateTime(2025, 11, 13, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = Guid.NewGuid(),
                Model = "Honda Civic Touring 1.5 Turbo",
                Price = 120000.00m,
                CreatedAt = new DateTime(2025, 11, 13, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}

