using System.Diagnostics;
using ContosoAutoTech.Infrastructure.AiSearch;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Data;

namespace ContosoAutoTech.Application.ContextProviders;

public class CustomerPoliciesContextProvider(AiSearchService aiSearchService)
{
    public Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider> CreateProviderFactory()
    {
        return ctx =>
        {
            TextSearchProviderOptions textSearchOptions = new()
            {
                SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke
            };

            return new TextSearchProvider(
                (query, cancellationToken) => SearchAdapter(query, cancellationToken, aiSearchService),
                ctx.SerializedState,
                ctx.JsonSerializerOptions,
                textSearchOptions);
        };
    }
    
    private static async Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchAdapter(
        string query, 
        CancellationToken cancellationToken,
        object aiSearchService)
    {
        // Call the AiSearchService.SearchCustomerPoliciesAsync method
        var method = aiSearchService.GetType().GetMethod("SearchCustomerPoliciesAsync");
        if (method == null)
            return [];

        var task = method.Invoke(aiSearchService, [query, cancellationToken]);
        
        // The method returns Task<IEnumerable<SearchResult>>
        // We need to convert SearchResult to TextSearchProvider.TextSearchResult
        dynamic? taskResult = task;
        if (taskResult == null)
            return [];

        await taskResult;
        var searchResults = taskResult.Result;

        var textSearchResults = new List<TextSearchProvider.TextSearchResult>();
        foreach (var result in searchResults)
        {
            textSearchResults.Add(new TextSearchProvider.TextSearchResult
            {
                SourceName = result.SourceName,
                SourceLink = result.SourceLink,
                Text = result.Text
            });
        }

        return textSearchResults;
    }
}

