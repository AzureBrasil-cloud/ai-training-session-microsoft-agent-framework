using ContosoAutoTech.Infrastructure.AiSearch.Models;
using Microsoft.Extensions.Logging;
using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Agents;
using Azure.Search.Documents.Agents.Models;
using Azure.Search.Documents.Models;

namespace ContosoAutoTech.Infrastructure.AiSearch;

public class AiSearchService(ILogger<AiSearchService> logger)
{
    public async Task<string> SearchCustomerPoliciesAsync(
        string query,
        AiSearchCredentials credentials,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Searching customer policies with query: {Query}", query);

        var credential = new AzureKeyCredential(credentials.SearchApiKey);

        // Ensure Knowledge Source and Agent exist
        await EnsureKnowledgeInfrastructureAsync(credentials, credential);

        // Use agentic retrieval to fetch results
        var agentClient = new KnowledgeAgentRetrievalClient(
            endpoint: new Uri(credentials.SearchEndpoint),
            agentName: credentials.KnowledgeAgentName,
            credential: credential
        );

        var messages = new List<KnowledgeAgentMessage>
        {
            new(content: [new KnowledgeAgentMessageTextContent(query)])
            {
                Role = "user"
            }
        };

        var retrievalRequest = new KnowledgeAgentRetrievalRequest(messages: messages);
        var retrievalResult = await agentClient.RetrieveAsync(retrievalRequest);

        // Extract the response text from the Knowledge Agent
        var responseContent = retrievalResult.Value.Response[0].Content[0] as KnowledgeAgentMessageTextContent;
        var responseText = responseContent?.Text ?? string.Empty;

        logger.LogInformation("Knowledge Agent response received: {ResponseLength} characters", responseText.Length);
        
        return responseText;
    }

    private async Task EnsureKnowledgeInfrastructureAsync(
        AiSearchCredentials credentials,
        AzureKeyCredential credential)
    {
        var indexClient = new SearchIndexClient(new Uri(credentials.SearchEndpoint), credential);

        // Create or update knowledge source
        var indexKnowledgeSource = new SearchIndexKnowledgeSource(
            name: credentials.KnowledgeSourceName,
            searchIndexParameters: new SearchIndexKnowledgeSourceParameters(searchIndexName: credentials.IndexName)
            {
                SourceDataSelect = "chunk_id,parent_id,chunk,title"
            }
        );

        await indexClient.CreateOrUpdateKnowledgeSourceAsync(indexKnowledgeSource);
        logger.LogInformation("Knowledge source '{KnowledgeSourceName}' created or updated successfully", credentials.KnowledgeSourceName);

        // Create or update knowledge agent
        var openAiParameters = new AzureOpenAIVectorizerParameters
        {
            ResourceUri = new Uri(credentials.AoaiEndpoint),
            DeploymentName = credentials.AoaiDeployment,
            ModelName = credentials.AoaiModel,
            ApiKey = credentials.AoaiApiKey
        };

        var agentModel = new KnowledgeAgentAzureOpenAIModel(azureOpenAIParameters: openAiParameters);
        var outputConfig = new KnowledgeAgentOutputConfiguration
        {
            Modality = KnowledgeAgentOutputConfigurationModality.AnswerSynthesis,
            IncludeActivity = true
        };

        var agent = new KnowledgeAgent(
            name: credentials.KnowledgeAgentName,
            models: [agentModel],
            knowledgeSources:
            [
                new KnowledgeSourceReference(credentials.KnowledgeSourceName)
                {
                    IncludeReferences = true,
                    IncludeReferenceSourceData = true,
                    RerankerThreshold = 2.5f
                }
            ]
        )
        {
            OutputConfiguration = outputConfig
        };

        await indexClient.CreateOrUpdateKnowledgeAgentAsync(agent);
        logger.LogInformation("Knowledge agent '{KnowledgeAgentName}' created or updated successfully", credentials.KnowledgeAgentName);
    }
}

