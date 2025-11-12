using System.ComponentModel;
using CarPriceMcp.Models;
using ModelContextProtocol.Server;

namespace CarPriceMcp;
[McpServerToolType]
public class PriceTools(ILogger<PriceTools> logger)
{
    [McpServerTool]
    [Description("Retrieve price by product code")]
    public Task<Price> GetPriceByProductCode(
        [Description("Product code. Full match. Required")] string code)
    {
        try
        {
            logger.LogInformation("Buscando preço do produto com código: {Code}", code);

            var prices = Prices.Items;

            if (!string.IsNullOrEmpty(code))
            {
                prices = prices.Where(p =>
                    string.Equals(p.ProductCode, code, StringComparison.CurrentCultureIgnoreCase));
            }

            var stock = prices.FirstOrDefault();

            return stock == null ? Task.FromResult(new Price()) : Task.FromResult(stock);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar preço para o produto {Code}", code);
            return null;
        }
    }
}