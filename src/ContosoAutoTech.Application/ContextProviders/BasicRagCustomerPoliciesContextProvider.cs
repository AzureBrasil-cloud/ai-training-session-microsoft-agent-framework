using System.Diagnostics;
using ContosoAutoTech.Infrastructure.AiSearch;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Data;

namespace ContosoAutoTech.Application.ContextProviders;

public class BasicRagCustomerPoliciesContextProvider(BasicRagService basicRagService)
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
        activity?.SetTag("search.provider", "Mock");
        
        var searchResults = await basicRagService.SearchCustomerPoliciesAsync(query, cancellationToken);
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

        activity?.SetTag("search.results.count", textSearchResults.Count);
        
        if (textSearchResults.Any())
        {
            activity?.SetTag("search.results.sources", string.Join(", ", textSearchResults.Select(r => r.SourceName)));
            
            // Add each result details
            for (int i = 0; i < textSearchResults.Count; i++)
            {
                var result = textSearchResults[i];
                activity?.SetTag($"search.result[{i}].source_name", result.SourceName);
                activity?.SetTag($"search.result[{i}].source_link", result.SourceLink);
                activity?.SetTag($"search.result[{i}].text", result.Text);
            }
        }
        
        return textSearchResults;
    }
}


