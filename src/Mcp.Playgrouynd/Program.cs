using Azure.AI.OpenAI;
using Azure.Identity;
using dotenv.net;
using Microsoft.Agents.AI;
using ModelContextProtocol.Client;
using OpenAI;

DotEnv.Load();
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set.");
var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-4o-mini";


/*
await using var mcpClient = await McpClientFactory.CreateAsync(
    new StdioClientTransport(new StdioClientTransportOptions
    {
        Name = "MyMcpServer",
        Command = "dotnet",
        Arguments = [
            "run",
            "--project",
            "../../../../CarPriceMcp/CarPriceMcp.csproj"
        ]
    })
);


var tools = await mcpClient.ListToolsAsync().ConfigureAwait(false);


// Create the chat client and agent, and provide the function tool to the agent.
AIAgent agent = new AzureOpenAIClient(
        new Uri(endpoint),
        new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions: "You are a helpful assistant", 
        tools: [.. tools]);

Console.WriteLine(await agent.RunAsync("Me fala a lista de carros disponiveis."));
*/

await using var mcpHttpClient = await McpClientFactory.CreateAsync(
    new HttpClientTransport(new HttpClientTransportOptions()
    {
        Name = "MyMcpClient",
        Endpoint = new Uri("http://localhost:5122/mcp")
    })
);


var tools2 = await mcpHttpClient.ListToolsAsync().ConfigureAwait(false);

// Create the chat client and agent, and provide the function tool to the agent.
AIAgent agent2 = new AzureOpenAIClient(
        new Uri(endpoint),
        new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions: "You are a helpful assistant", 
        tools: [.. tools2]);

Console.WriteLine(await agent2.RunAsync("Lista todo o estoque de peças disponíveis com quantidades, status e localização."));
