using Azure;
using Azure.AI.OpenAI;
using ContosoAutoTech.Infrastructure.Azure.Shared;

namespace ContosoAutoTech.Infrastructure.AiInference;

public partial class AiInferenceService
{
    private AzureOpenAIClient CreateClient(ApiKeyCredentials credentials)
    {
        return new AzureOpenAIClient(new Uri(
                credentials.Endpoint), 
            new AzureKeyCredential(credentials.Key));
    }
}