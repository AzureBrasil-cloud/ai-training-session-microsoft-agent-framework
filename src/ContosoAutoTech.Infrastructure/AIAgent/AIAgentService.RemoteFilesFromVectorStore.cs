// using ContosoAutoTech.Infrastructure.Azure.Shared;
// using ContosoAutoTech.Infrastructure.Shared;
//
// namespace ContosoAutoTech.Infrastructure.AIAgent;
//
// public partial class AiAgentService
// {
//     public virtual async Task RemoveFilesFromVectorStoreAsync(
//         Credentials credentials, 
//         string vectorStoreId, 
//         IEnumerable<string> fileIds)
//     {
//         var client = CreateAgentsClient(credentials);
//
//         foreach (var fileId in fileIds)
//         {
//             await client.DeleteVectorStoreFileAsync(vectorStoreId, fileId);
//         }
//     }
// }