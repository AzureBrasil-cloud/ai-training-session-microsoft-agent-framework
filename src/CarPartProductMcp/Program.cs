var builder = WebApplication.CreateBuilder(args);

// Add MCP services and configure the HTTP transport
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly(typeof(Program).Assembly); // Configures the server to use HTTP transport

var app = builder.Build();

// Middleware de validação da chave
app.Use(async (context, next) =>
{
    // MCP usa sempre POST em um endpoint, normalmente /mcp
    if (context.Request.Path.StartsWithSegments("/mcp"))
    {
        if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey) ||
            apiKey != "123")
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }
    }

    await next();
});

// Map the MCP server endpoint
app.MapMcp("/mcp"); 

app.Run();