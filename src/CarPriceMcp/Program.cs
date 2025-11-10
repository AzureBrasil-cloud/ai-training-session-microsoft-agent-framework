using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using CarPriceMcp;

var builder = Host.CreateApplicationBuilder(args);

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Mudei para Debug

// Registrar serviços
builder.Services.AddSingleton<CarPartsService>();
builder.Services.AddSingleton<CarPartsTools>();

// Configurar MCP Server
builder.Services.AddMcpServer(options =>
    {
        options.ServerInfo = new()
        {
            Name = "car-catalog-server",
            Version = "1.0.0"
        };
    })
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(CarPartsTools).Assembly);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🚗 MCP Car Catalog Server iniciado!");

await app.RunAsync();