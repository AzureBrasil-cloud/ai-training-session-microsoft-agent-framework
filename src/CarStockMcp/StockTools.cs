using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace CarStockMcp;

[McpServerToolType]
public class StockTools(ILogger<StockTools> logger)
{
    [McpServerTool]
    [Description("Retrieve stock by product code")]
    public Task<Stock> GetStockByProductCode(
        [Description("Product code. Full match. Required")] string code)
    {
        try
        {
            logger.LogInformation("Buscando estoque do produto com código: {Code}", code);

            var stocks = Stocks.Items;

            if (!string.IsNullOrEmpty(code))
            {
                stocks = stocks.Where(p =>
                    string.Equals(p.ProductCode, code, StringComparison.CurrentCultureIgnoreCase));
            }

            var stock = stocks.FirstOrDefault();

            return stock == null ? Task.FromResult(new Stock()) : Task.FromResult(stock);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar estoque para o produto {Code}", code);
            return null;
        }
    }
}
