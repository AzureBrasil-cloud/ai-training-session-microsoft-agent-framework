using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace CarPriceMcp;

[McpServerToolType]
public class CarPartsTools(CarPartsService partsService, ILogger<CarPartsTools> logger)
{
    [McpServerTool]
    [Description("Lista todas as peças disponíveis no catálogo com preços, marcas, modelos e categorias")]
    public Task<string> ListAllParts()
    {
        logger.LogInformation("Listando todas as peças do catálogo");

        var parts = partsService.GetAllParts();
        var partsList = parts.Select(p => new
        {
            nome = p.Name,
            marca = p.Brand,
            modelo = p.Model,
            categoria = p.Category,
            preco = $"R$ {p.Price:N2}"
        }).ToList();

        var jsonResult = JsonSerializer.Serialize(partsList, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        var result = $"📦 Catálogo de Peças ({parts.Count} itens):\n\n{jsonResult}";
        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista todas as peças disponíveis para uma marca específica")]
    public Task<string> ListPartsByBrand(
        [Description("A marca do veículo (ex: Honda, Toyota, Chevrolet, Volkswagen, Hyundai)")]
        string brand)
    {
        logger.LogInformation("Buscando peças da marca: {Brand}", brand);

        if (string.IsNullOrWhiteSpace(brand))
        {
            return Task.FromResult("❌ Erro: O parâmetro 'brand' é obrigatório.");
        }

        var parts = partsService.GetPartsByBrand(brand).ToList();

        string result;
        if (parts.Any())
        {
            logger.LogInformation("Encontradas {Count} peças para a marca {Brand}", parts.Count, brand);
            
            var partsList = parts.Select(p => new
            {
                nome = p.Name,
                modelo = p.Model,
                categoria = p.Category,
                preco = $"R$ {p.Price:N2}"
            }).ToList();

            var jsonResult = JsonSerializer.Serialize(partsList, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            result = $"🔧 Peças para {brand} ({parts.Count} itens):\n\n{jsonResult}";
        }
        else
        {
            logger.LogWarning("Nenhuma peça encontrada para a marca: {Brand}", brand);
            var availableBrands = string.Join(", ", partsService.GetAvailableBrands());
            result = $"❌ Nenhuma peça encontrada para a marca '{brand}'.\n\n" +
                     $"Marcas disponíveis:\n{availableBrands}";
        }

        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista todas as peças disponíveis para um modelo específico de veículo")]
    public Task<string> ListPartsByModel(
        [Description("O modelo do veículo (ex: Civic, Corolla, Onix, Gol, HB20)")]
        string model)
    {
        logger.LogInformation("Buscando peças do modelo: {Model}", model);

        if (string.IsNullOrWhiteSpace(model))
        {
            return Task.FromResult("❌ Erro: O parâmetro 'model' é obrigatório.");
        }

        var parts = partsService.GetPartsByModel(model).ToList();

        string result;
        if (parts.Any())
        {
            logger.LogInformation("Encontradas {Count} peças para o modelo {Model}", parts.Count, model);
            
            var partsList = parts.Select(p => new
            {
                nome = p.Name,
                marca = p.Brand,
                categoria = p.Category,
                preco = $"R$ {p.Price:N2}"
            }).ToList();

            var jsonResult = JsonSerializer.Serialize(partsList, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            result = $"🚗 Peças para {model} ({parts.Count} itens):\n\n{jsonResult}";
        }
        else
        {
            logger.LogWarning("Nenhuma peça encontrada para o modelo: {Model}", model);
            var availableModels = string.Join(", ", partsService.GetAvailableModels());
            result = $"❌ Nenhuma peça encontrada para o modelo '{model}'.\n\n" +
                     $"Modelos disponíveis:\n{availableModels}";
        }

        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista todas as marcas de veículos disponíveis no catálogo")]
    public Task<string> ListAvailableBrands()
    {
        logger.LogInformation("Listando marcas disponíveis");

        var brands = partsService.GetAvailableBrands().ToList();
        var result = $"🏭 Marcas disponíveis ({brands.Count}):\n\n" +
                     string.Join("\n", brands.Select(b => $"• {b}"));

        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista todos os modelos de veículos disponíveis no catálogo")]
    public Task<string> ListAvailableModels()
    {
        logger.LogInformation("Listando modelos disponíveis");

        var models = partsService.GetAvailableModels().ToList();
        var result = $"🚙 Modelos disponíveis ({models.Count}):\n\n" +
                     string.Join("\n", models.Select(m => $"• {m}"));

        return Task.FromResult(result);
    }
}