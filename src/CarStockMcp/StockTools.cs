using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace CarStockMcp;

[McpServerToolType]
public class StockTools(StockService stockService, ILogger<StockTools> logger)
{
    [McpServerTool]
    [Description("Lista todo o estoque de peças disponíveis com quantidades, status e localização")]
    public Task<string> ListAllStock()
    {
        logger.LogInformation("Listando todo o estoque");

        var stock = stockService.GetAllStock();
        var stockList = stock.Select(s => new
        {
            id = s.PartId,
            nome = s.Part.Name,
            marca = s.Part.Brand,
            modelo = s.Part.Model,
            categoria = s.Part.Category,
            preco = $"R$ {s.Part.Price:N2}",
            quantidade = s.Quantity,
            estoqueMinimo = s.MinimumStock,
            status = s.StockStatus,
            localizacao = s.Location,
            ultimaAtualizacao = s.LastUpdated.ToString("dd/MM/yyyy HH:mm")
        }).ToList();

        var jsonResult = JsonSerializer.Serialize(stockList, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        var result = $"📦 Estoque Total ({stock.Count()} itens):\n\n{jsonResult}";
        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista o estoque de peças filtrado por marca específica")]
    public Task<string> ListStockByBrand(
        [Description("A marca do veículo (ex: Honda, Toyota, Chevrolet, Volkswagen, Hyundai)")]
        string brand)
    {
        logger.LogInformation("Listando estoque da marca: {Brand}", brand);

        if (string.IsNullOrWhiteSpace(brand))
        {
            return Task.FromResult("❌ Erro: O parâmetro 'brand' é obrigatório.");
        }

        var stock = stockService.GetStockByBrand(brand).ToList();

        string result;
        if (stock.Any())
        {
            logger.LogInformation("Encontrados {Count} itens para a marca {Brand}", stock.Count, brand);
            
            var stockList = stock.Select(s => new
            {
                id = s.PartId,
                nome = s.Part.Name,
                modelo = s.Part.Model,
                quantidade = s.Quantity,
                status = s.StockStatus,
                localizacao = s.Location
            }).ToList();

            var jsonResult = JsonSerializer.Serialize(stockList, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            result = $"🔧 Estoque - {brand} ({stock.Count} itens):\n\n{jsonResult}";
        }
        else
        {
            logger.LogWarning("Nenhum item encontrado para a marca: {Brand}", brand);
            var availableBrands = string.Join(", ", stockService.GetAvailableBrands());
            result = $"❌ Nenhuma peça encontrada para a marca '{brand}'.\n\n" +
                     $"Marcas disponíveis:\n{availableBrands}";
        }

        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista o estoque de peças filtrado por modelo específico de veículo")]
    public Task<string> ListStockByModel(
        [Description("O modelo do veículo (ex: Civic, Corolla, Onix, Gol, HB20)")]
        string model)
    {
        logger.LogInformation("Listando estoque do modelo: {Model}", model);

        if (string.IsNullOrWhiteSpace(model))
        {
            return Task.FromResult("❌ Erro: O parâmetro 'model' é obrigatório.");
        }

        var stock = stockService.GetStockByModel(model).ToList();

        string result;
        if (stock.Any())
        {
            logger.LogInformation("Encontrados {Count} itens para o modelo {Model}", stock.Count, model);
            
            var stockList = stock.Select(s => new
            {
                id = s.PartId,
                nome = s.Part.Name,
                marca = s.Part.Brand,
                quantidade = s.Quantity,
                status = s.StockStatus,
                localizacao = s.Location
            }).ToList();

            var jsonResult = JsonSerializer.Serialize(stockList, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            result = $"🚗 Estoque - {model} ({stock.Count} itens):\n\n{jsonResult}";
        }
        else
        {
            logger.LogWarning("Nenhum item encontrado para o modelo: {Model}", model);
            var availableModels = string.Join(", ", stockService.GetAvailableModels());
            result = $"❌ Nenhuma peça encontrada para o modelo '{model}'.\n\n" +
                     $"Modelos disponíveis:\n{availableModels}";
        }

        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista todas as peças com estoque baixo (quantidade igual ou menor que o estoque mínimo)")]
    public Task<string> ListLowStock()
    {
        logger.LogInformation("Listando itens com estoque baixo");

        var stock = stockService.GetLowStock().ToList();

        if (!stock.Any())
        {
            return Task.FromResult("✅ Nenhum item com estoque baixo no momento!");
        }

        var stockList = stock.Select(s => new
        {
            id = s.PartId,
            nome = s.Part.Name,
            marca = s.Part.Brand,
            modelo = s.Part.Model,
            quantidadeAtual = s.Quantity,
            estoqueMinimo = s.MinimumStock,
            diferenca = s.MinimumStock - s.Quantity,
            localizacao = s.Location,
            status = s.StockStatus
        }).ToList();

        var jsonResult = JsonSerializer.Serialize(stockList, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        var result = $"⚠️ Alerta de Estoque Baixo ({stock.Count} itens):\n\n{jsonResult}";
        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista todas as peças que estão completamente esgotadas (quantidade = 0)")]
    public Task<string> ListOutOfStock()
    {
        logger.LogInformation("Listando itens esgotados");

        var stock = stockService.GetOutOfStock().ToList();

        if (!stock.Any())
        {
            return Task.FromResult("✅ Nenhum item esgotado no momento!");
        }

        var stockList = stock.Select(s => new
        {
            id = s.PartId,
            nome = s.Part.Name,
            marca = s.Part.Brand,
            modelo = s.Part.Model,
            estoqueMinimo = s.MinimumStock,
            diasSemEstoque = (DateTime.Now - s.LastUpdated).Days,
            localizacao = s.Location
        }).ToList();

        var jsonResult = JsonSerializer.Serialize(stockList, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        var result = $"🚨 Itens Esgotados ({stock.Count} itens):\n\n{jsonResult}";
        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Busca informações detalhadas de estoque de uma peça específica pelo ID")]
    public Task<string> GetStockByPartId(
        [Description("O ID da peça no sistema")]
        int partId)
    {
        logger.LogInformation("Buscando estoque da peça ID: {PartId}", partId);

        var stock = stockService.GetStockByPartId(partId);

        string result;
        if (stock != null)
        {
            var stockInfo = new
            {
                id = stock.PartId,
                peca = new
                {
                    nome = stock.Part.Name,
                    marca = stock.Part.Brand,
                    modelo = stock.Part.Model,
                    categoria = stock.Part.Category,
                    preco = $"R$ {stock.Part.Price:N2}"
                },
                estoque = new
                {
                    quantidade = stock.Quantity,
                    estoqueMinimo = stock.MinimumStock,
                    status = stock.StockStatus,
                    localizacao = stock.Location,
                    ultimaAtualizacao = stock.LastUpdated.ToString("dd/MM/yyyy HH:mm")
                }
            };

            var jsonResult = JsonSerializer.Serialize(stockInfo, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            result = $"📋 Detalhes da Peça:\n\n{jsonResult}";
        }
        else
        {
            logger.LogWarning("Peça não encontrada: ID {PartId}", partId);
            result = $"❌ Peça com ID {partId} não encontrada no sistema.";
        }

        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Atualiza a quantidade em estoque de uma peça específica")]
    public Task<string> UpdateStock(
        [Description("O ID da peça a ser atualizada")]
        int partId,
        [Description("A nova quantidade em estoque")]
        int quantity)
    {
        logger.LogInformation("Atualizando estoque - Peça ID: {PartId}, Nova quantidade: {Quantity}", partId, quantity);

        if (quantity < 0)
        {
            return Task.FromResult("❌ Erro: A quantidade não pode ser negativa.");
        }

        var success = stockService.UpdateStock(partId, quantity);

        string result;
        if (success)
        {
            var stock = stockService.GetStockByPartId(partId);
            logger.LogInformation("Estoque atualizado com sucesso - {Name}: {Quantity}", stock.Part.Name, stock.Quantity);
            
            result = $"✅ Estoque atualizado com sucesso!\n\n" +
                     $"Peça: {stock.Part.Name} ({stock.Part.Brand} {stock.Part.Model})\n" +
                     $"Quantidade Nova: {stock.Quantity}\n" +
                     $"Estoque Mínimo: {stock.MinimumStock}\n" +
                     $"Status: {stock.StockStatus}\n" +
                     $"Localização: {stock.Location}";
        }
        else
        {
            logger.LogWarning("Falha ao atualizar estoque - Peça ID {PartId} não encontrada", partId);
            result = $"❌ Erro: Peça com ID {partId} não encontrada no sistema.";
        }

        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista todas as marcas de veículos disponíveis no catálogo")]
    public Task<string> ListAvailableBrands()
    {
        logger.LogInformation("Listando marcas disponíveis");

        var brands = stockService.GetAvailableBrands().ToList();
        var result = $"🏭 Marcas Disponíveis ({brands.Count}):\n\n" +
                     string.Join("\n", brands.Select(b => $"• {b}"));

        return Task.FromResult(result);
    }

    [McpServerTool]
    [Description("Lista todos os modelos de veículos disponíveis no catálogo")]
    public Task<string> ListAvailableModels()
    {
        logger.LogInformation("Listando modelos disponíveis");

        var models = stockService.GetAvailableModels().ToList();
        var result = $"🚙 Modelos Disponíveis ({models.Count}):\n\n" +
                     string.Join("\n", models.Select(m => $"• {m}"));

        return Task.FromResult(result);
    }
}