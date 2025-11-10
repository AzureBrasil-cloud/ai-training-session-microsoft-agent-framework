// using Azure.AI.Projects;
// using ContosoAutoTech.Infrastructure.Azure.Shared;
// using ContosoAutoTech.Infrastructure.Shared;
// using VectorStore = ContosoAutoTech.Infrastructure.AIAgent.Models.VectorStore;
//
// namespace ContosoAutoTech.Infrastructure.AIAgent;
//
// using VectorStore = Models.VectorStore;
//
// public partial class AiAgentService
// {
//     public virtual async Task<VectorStore> CreateVectorStoreAsync(
//         Credentials credentials, 
//         string name,
//         IEnumerable<string> files)
//     {
//         var client = CreateAgentsClient(credentials);
//
//         var vectorStore = await client.CreateVectorStoreAsync(
//             fileIds:  files,
//             name: name,
//             expiresAfter: new VectorStoreExpirationPolicy(VectorStoreExpirationPolicyAnchor.LastActiveAt, 2));
//
//         return new VectorStore(vectorStore.Value.Id, vectorStore.Value.Name);
//     }
// }