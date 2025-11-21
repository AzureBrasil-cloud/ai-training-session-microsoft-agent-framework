using System.ComponentModel;
using CarPartProductRelated.Api.Models;
using ModelContextProtocol.Server;

namespace CarPartProductRelated.Api;

[McpServerToolType]
public class RecommendationTools(ILogger<RecommendationTools> logger)
{

    [McpServerTool]
    [Description("Retrieve Product Recomendation by product code")]
    public Task<RecommendationResponse?> GetProductRecommendation(
        [Description("Product code. Full match. Required")] string code)
    {
        try
        {
            var products = ProductDatabase.Items.ToList();

            var product = products.FirstOrDefault(p => p.ProductCode.Equals(code, StringComparison.OrdinalIgnoreCase));

            if (product is null)
            {
                return Task.FromResult<RecommendationResponse?>(null);
            }

            var related = products
                .Where(p =>
                    p.ProductCode != product.ProductCode &&
                    (p.Brand == product.Brand || p.Model == product.Model))
                .ToList();

            return Task.FromResult(new RecommendationResponse(
                product.ProductCode,
                product.Name,
                related
            ))!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar recomendações para o produto {Code}", code);
            return Task.FromResult<RecommendationResponse?>(null);
        }
    }
}