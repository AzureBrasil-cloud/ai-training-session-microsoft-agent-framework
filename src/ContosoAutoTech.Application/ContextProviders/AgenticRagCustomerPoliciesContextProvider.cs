using System.Diagnostics;
using ContosoAutoTech.Infrastructure.AiSearch;
using ContosoAutoTech.Infrastructure.AiSearch.Models;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Data;
using Microsoft.Extensions.Configuration;

namespace ContosoAutoTech.Application.ContextProviders;

public class AgenticRagCustomerPoliciesContextProvider(AiSearchService aiSearchService, IConfiguration configuration)
{
    private static readonly ActivitySource ActivitySource = InstrumentationConfig.ActivitySource;
    
    public Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider> CreateProviderFactory()
    {
        return ctx =>
        {
            TextSearchProviderOptions textSearchOptions = new()
            {
                SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke
            };

            return new TextSearchProvider(
                (query, cancellationToken) => SearchAdapter(query, cancellationToken),
                ctx.SerializedState,
                ctx.JsonSerializerOptions,
                textSearchOptions);
        };
    }
    
    private async Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchAdapter(
        string query, 
        CancellationToken cancellationToken)
    {
        using Activity? activity = ActivitySource.StartActivity();
        activity?.SetTag("search.query", query);
        activity?.SetTag("search.provider", "KnowledgeAgent");
        
        var credentials = GetCredentials();
        var responseText = await aiSearchService.SearchCustomerPoliciesAsync(query, credentials, cancellationToken);
        
        // Return the Knowledge Agent's response as a single search result
        var textSearchResults = new List<TextSearchProvider.TextSearchResult>
        {
            new()
            {
                SourceName = "Customer Policies Knowledge Agent",
                SourceLink = "azure-ai-search",
                Text = responseText
            }
        };

        activity?.SetTag("search.response.length", responseText.Length);
        activity?.SetTag("search.response.text", responseText);
        
        return textSearchResults;
    }
    
    private AiSearchCredentials GetCredentials()
    {
        return new AiSearchCredentials
        {
            SearchEndpoint = configuration["AiSearch:SearchEndpoint"]!,
            SearchApiKey = configuration["AiSearch:SearchApiKey"]!,
            IndexName = configuration["AiSearch:IndexName"]!,
            KnowledgeSourceName = configuration["AiSearch:KnowledgeSourceName"]!,
            KnowledgeAgentName = configuration["AiSearch:KnowledgeAgentName"]!,
            AoaiEndpoint = configuration["AiSearch:AoaiEndpoint"]!,
            AoaiApiKey = configuration["AiSearch:AoaiApiKey"]!,
            AoaiDeployment = configuration["AiSearch:AoaiDeployment"]!,
            AoaiModel = configuration["AiSearch:AoaiModel"]!
        };
    }
}

