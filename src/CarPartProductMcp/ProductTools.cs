using System.ComponentModel;
using CarPartProduct.Models;
using ModelContextProtocol.Server;

namespace CarPartProduct;

[McpServerToolType]
public class ProductTools(ILogger<ProductTools> logger)
{
    /// <summary>
    /// Retrieve all products by params
    /// </summary>
    [McpServerTool]
    [Description("Retrieve all products filtered by name or code. Returns matching products or all if no filters provided.")]
    public IEnumerable<Product> GetAllProductsByParam(
        [Description("Product name. Partial match. Optional")] string? name = null,
        [Description("Product code. Partial match. Optional")] string? code = null)
    {
        logger.LogInformation("Fetching products with filters - Name: {Name}, Code: {Code}", name, code);

        try
        {
            var products = Products.Items.AsEnumerable();
        
            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
        
            if (!string.IsNullOrEmpty(code))
            {
                products = products.Where(p => p.ProductCode.Contains(code, StringComparison.OrdinalIgnoreCase));
            }
        
            return products;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving products");
            throw new InvalidOperationException("It was not possible to retrieve the list of products", ex);
        }
    }
}

