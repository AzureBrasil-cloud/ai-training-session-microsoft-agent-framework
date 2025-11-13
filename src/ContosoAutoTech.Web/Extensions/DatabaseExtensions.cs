using ContosoAutoTech.Data;
using ContosoAutoTech.Data.Initialization;

namespace ContosoAutoTech.Web.Extensions;

public static class DatabaseExtensions
{
    public static async Task<IApplicationBuilder> UseDatabaseSeeding(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DatabaseSeeder.SeedDatabaseAsync(context);
        return app;
    }
}

