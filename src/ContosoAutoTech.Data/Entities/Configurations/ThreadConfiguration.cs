using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContosoAutoTech.Data.Entities.Configurations;

public class ThreadConfiguration : IEntityTypeConfiguration<Thread>
{
    public void Configure(EntityTypeBuilder<Thread> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.State)
            .IsRequired();
    }
}
