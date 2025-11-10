using ContosoAutoTech.Infrastructure.Azure.Shared;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService
{
    public virtual async Task AddFilesToVectorStoreAsync(
        Credentials credentials, 
        string vectorStoreId, 
        IEnumerable<string> fileIds)
    {
        var client = CreateAgentsClient(credentials);

        foreach (var fileId in fileIds)
        {
            await client.CreateVectorStoreFileAsync(vectorStoreId, fileId);
        }
    }
}