using Azure.AI.OpenAI;
using Azure.Identity;
using dotenv.net;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using OpenAI;

DotEnv.Load();
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set.");
var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-4o-mini";
/*
await using var mcpClient = await McpClient.CreateAsync(
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
*/

await using var mcpClient = await McpClient.CreateAsync(
    new HttpClientTransport(new HttpClientTransportOptions()
    {
        Name = "StockCarPartMcp",
        Endpoint = new Uri("http://localhost:5000/mcp")
    })
);


var tools = await mcpClient.ListToolsAsync().ConfigureAwait(false);

// Create the chat client and agent, and provide the function tool to the agent.
AIAgent agent = new AzureOpenAIClient(
        new Uri(endpoint),
        new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions: "Voce é um assistente inteligente que consulta apenas as ferramentas para informar os precos.", 
        tools: [.. tools]);

Console.WriteLine(await agent.RunAsync("Me fala a lista de carros disponiveis."));

await using var mcpHttpClient = await McpClient.CreateAsync(
    new HttpClientTransport(new HttpClientTransportOptions()
    {
        Name = "StockCarPartMcp",
        Endpoint = new Uri("http://localhost:5122/mcp")
    })
);

var tools2 = await mcpHttpClient.ListToolsAsync().ConfigureAwait(false);

// Create the chat client and agent, and provide the function tool to the agent.
AIAgent agent2 = new AzureOpenAIClient(
        new Uri(endpoint),
        new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions: "Você é um assistente que utiliza as ferramentas disponiveis para consultar Estoque.", 
        tools: [.. tools2]);

Console.WriteLine(await agent2.RunAsync("Lista todo o estoque de peças disponíveis com quantidades, status e localização."));

var orchestrator = new AzureOpenAIClient(
        new Uri(endpoint),
        new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions:
        "Você é um orquestrador. Nunca combine dados de outros agentes a menos que solicitado explicitamente. Selecione o agente a ser chamado de acordo com a solicitacao do cliente, se tem haver com estoque chame o agente de estoque");

var workflow = new WorkflowBuilder(orchestrator)
    .AddEdge(orchestrator, agent)
    .AddEdge(orchestrator, agent2)
    .Build();

var workFlowAgent = workflow.AsAgent();
var thread = workFlowAgent.GetNewThread();

var result = await workFlowAgent.RunAsync("Liste todas as pecas em estoque", thread);


// Execute the workflow
/*
await using StreamingRun run = await InProcessExecution.StreamAsync(workflow, new ChatMessage(ChatRole.User, "Liste todas as pecas em estoque"));

await run.TrySendMessageAsync(new TurnToken(emitEvents: true));
await foreach (WorkflowEvent evt in run.WatchStreamAsync())
{
    if (evt is AgentRunUpdateEvent executorComplete)
    {
        Console.Write($"{executorComplete.Data}");
    }
}*/
