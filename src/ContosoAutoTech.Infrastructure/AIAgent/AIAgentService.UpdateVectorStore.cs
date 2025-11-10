// using ContosoAutoTech.Infrastructure.Azure.Shared;
// using ContosoAutoTech.Infrastructure.Shared;
//
// namespace ContosoAutoTech.Infrastructure.AIAgent;
//
// public partial class AiAgentService
// {
//     public virtual async Task UpdateVectorStoreAsync(
//         Credentials credentials,
//         string vectorStoreId,
//         string name)
//     {
//         var client = CreateAgentsClient(credentials);
//         await client.ModifyVectorStoreAsync(vectorStoreId, name);
//     }
// }