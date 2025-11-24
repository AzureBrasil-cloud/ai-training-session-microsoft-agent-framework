using System.Collections.Concurrent;
using System.Reflection;
using DiscountMcp;
using DiscountMcp.Tools;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DiscountSessionStore>();
builder.Services.AddSingleton<ManagerDiscountTools>();
builder.Services.AddSingleton<DiscountTools>();

var toolCache = new ConcurrentDictionary<string, McpServerTool[]>();

// Add MCP services and configure the HTTP transport
builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        options.ConfigureSessionOptions = (httpContext, mcpOptions, cancellationToken) =>
        {
            var toolCategory = httpContext.Request.RouteValues["toolCategory"]?.ToString()?.ToLower() ?? "all";

            // Usa cache se já foi criado, senão cria e cacheia
            var tools = toolCache.GetOrAdd(toolCategory, category =>
            {
                var serviceProvider = httpContext.RequestServices;
                return GetToolsForCategory(category, serviceProvider);
            });
            
            if (tools.Length == 0) return Task.CompletedTask;
            
            mcpOptions.Capabilities = new ServerCapabilities
            {
                Tools = new ToolsCapability()
            };
            
            var toolCollection = mcpOptions.ToolCollection = [];

            foreach (var tool in tools)
            {
                toolCollection.Add(tool);
            }

            return Task.CompletedTask;
        };
    })
    .WithToolsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

app.MapMcp("/{toolCategory?}");

app.Run();
return;

static McpServerTool[] GetToolsForCategory(string category, IServiceProvider serviceProvider)
{
    return category switch
    {
        "manager" => GetToolsForType<ManagerDiscountTools>(serviceProvider),
        "discount" => GetToolsForType<DiscountTools>(serviceProvider),
        _ => Array.Empty<McpServerTool>()
    };
}

static McpServerTool[] GetToolsForType<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(
    System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicMethods)] T>(
    IServiceProvider serviceProvider)
{
    var tools = new List<McpServerTool>();
    var toolType = typeof(T);
    
    var instance = serviceProvider.GetRequiredService<T>();
    
    var methods = toolType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Where(m => m.GetCustomAttributes(typeof(McpServerToolAttribute), false).Any());

    foreach (var method in methods)
    {
        try
        {
            var tool = McpServerTool.Create(method, target: instance, new McpServerToolCreateOptions());
            tools.Add(tool);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add tool {toolType.Name}.{method.Name}: {ex.Message}");
        }
    }

    return [.. tools];
}