// using ContosoAutoTech.Infrastructure.Azure.Shared;
// using ContosoAutoTech.Infrastructure.Shared;
// using Thread = ContosoAutoTech.Infrastructure.AIAgent.Models.Thread;
//
// namespace ContosoAutoTech.Infrastructure.AIAgent;
//
// using Thread = Models.Thread;
//
// public partial class AiAgentService
// {
//     public virtual async Task<Thread> CreateThreadAsync(Credentials credentials)
//     {
//         var client = CreateAgentsClient(credentials);
//
//         var threadResponse = await client.CreateThreadAsync();
//
//         return new Thread(threadResponse.Value.Id);
//     }
// }