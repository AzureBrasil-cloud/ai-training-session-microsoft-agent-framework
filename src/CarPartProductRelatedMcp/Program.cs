var builder = WebApplication.CreateBuilder(args);

// Add MCP services and configure the HTTP transport
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly(typeof(Program).Assembly); // Configures the server to use HTTP transport

var app = builder.Build();

// Map the MCP server endpoint
app.MapMcp("/mcp"); 

app.Run();