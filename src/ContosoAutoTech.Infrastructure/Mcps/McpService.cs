using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

namespace ContosoAutoTech.Infrastructure.Mcps;

public partial class McpService
{
    public async Task<(IList<AITool> tools, McpClient mcpClient)> GetStdioMcpToolAsync(string name, 
        string command,
        IList<string>? arguments = null)
    {
        var mcpClient = await McpClient.CreateAsync(
            new StdioClientTransport(new StdioClientTransportOptions
            {
                Name = name,
                Command = command,
                Arguments = arguments
            })
        );
        var tools = await mcpClient.ListToolsAsync();

        return (tools.Select(AITool (x) => x).ToList(), mcpClient);
    }

    public async Task<(IList<AITool> tools, McpClient mcpClient)> GetHttpMcpToolAsync(string name, string endpoint)
    {
        var mcpHttpClient = await McpClient.CreateAsync(
            new HttpClientTransport(new HttpClientTransportOptions
            {
                Name = name,
                Endpoint = new Uri(endpoint)
            })
        );

        var tools = await mcpHttpClient.ListToolsAsync();
        return (tools.Select(AITool (x) => x).ToList(), mcpHttpClient);
    }
}