using Azure;
using Azure.AI.DocumentIntelligence;
using ContosoAutoTech.Infrastructure.Azure.Shared;

namespace ContosoAutoTech.Infrastructure.Azure.DocumentIntelligence;

public partial class DocumentIntelligenceService
{
    private DocumentIntelligenceClient CreateClient(ApiKeyCredentials credentials)
    {
         return new DocumentIntelligenceClient(new Uri(
                credentials.Endpoint), 
            new AzureKeyCredential(credentials.Key));
    }
}