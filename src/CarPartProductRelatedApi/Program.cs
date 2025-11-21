using CarPartProductRelated.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// -------------------
// Endpoint de recomendação
// -------------------
app.MapGet("/recommendations/{productCode}", (string productCode) =>
{
    var products = ProductDatabase.Items.ToList();

    var product = products.FirstOrDefault(p => p.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase));

    if (product is null)
    {
        return Results.NotFound(new { message = $"Product '{productCode}' not found." });
    }

    var related = products
        .Where(p =>
            p.ProductCode != product.ProductCode &&
            (p.Brand == product.Brand || p.Model == product.Model))
        .ToList();

    return Results.Ok(
        new RecommendationResponse(
            product.ProductCode,
            product.Name,
            related
        )
    );
});

// -------------------
app.MapGet("/", () => "Recommendation API using Brand/Model matching.");
app.Run();
