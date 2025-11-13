using ContosoAutoTech.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContosoAutoTech.Data.Initialization;

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(AppDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed CarReferencePrices if empty
        if (!await context.CarReferencePrices.AnyAsync())
        {
            await context.CarReferencePrices.AddRangeAsync(
                new CarReferencePrice("Toyota Corolla XEi 2.0", 110000.00m),
                new CarReferencePrice("Honda Civic Touring 1.5 Turbo", 120000.00m)
            );

            await context.SaveChangesAsync();
        }
    }
}

