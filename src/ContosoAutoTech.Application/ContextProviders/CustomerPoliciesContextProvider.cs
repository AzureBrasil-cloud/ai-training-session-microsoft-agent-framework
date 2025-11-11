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
        AiSearchService aiSearchService)
    {
        var searchResults = await aiSearchService.SearchCustomerPoliciesAsync(query, cancellationToken);
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

