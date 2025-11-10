// using Azure.AI.Projects;
// using ContosoAutoTech.Infrastructure.Azure.Shared;
// using ContosoAutoTech.Infrastructure.Shared;
//
// namespace ContosoAutoTech.Infrastructure.AIAgent;
//
// public partial class AiAgentService
// {
//     public virtual async Task ClearAgentToolsAsync(Credentials credentials, string agentId)
//     {
//         var client = CreateAgentsClient(credentials);
//
//         await client.UpdateAgentAsync(
//             agentId,
//             tools: new List<ToolDefinition>(),
//             toolResources: new ToolResources());
//     }
// }