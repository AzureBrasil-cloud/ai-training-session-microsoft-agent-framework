using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContosoAutoTech.Data.Entities.Configurations;

public class CarSaleConfiguration : IEntityTypeConfiguration<CarSale>
{
    public void Configure(EntityTypeBuilder<CarSale> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Model)
            .IsRequired();
        
        builder
            .Property(x => x.LicensePlate)
            .IsRequired();

        builder
            .Property(x => x.Color)
            .IsRequired();

        builder
            .Property(x => x.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder
            .Property(x => x.Description)
            .IsRequired();
        
        builder
            .Property(x => x.ImageUrl)
            .IsRequired();
        
        builder
            .Property(x => x.ReferencePrice)
            .HasPrecision(18, 2);
        
        builder
            .Property(x => x.AgentConsideration)
            .IsRequired();

        builder
            .OwnsOne(x => x.CarFeatures, features =>
            {
                features.Property(f => f.Strengths)
                    .IsRequired()
                    .HasConversion(
                        v => string.Join(';', v),
                        v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );

                features.Property(f => f.Weaknesses)
                    .IsRequired()
                    .HasConversion(
                        v => string.Join(';', v),
                        v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();
    }
}

