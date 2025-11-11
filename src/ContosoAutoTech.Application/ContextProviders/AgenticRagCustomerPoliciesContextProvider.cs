using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ContosoAutoTech.Infrastructure.AiSearch;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Data;
using Microsoft.Extensions.Configuration;
using Azure.Search.Documents.Agents.Models;
using ContosoAutoTech.Infrastructure.Shared;

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
        var retrievalResult = await aiSearchService.SearchCustomerPoliciesAsync(query, credentials, cancellationToken);
        
        var sb = new StringBuilder();

        // Response
        sb.AppendLine("Response:");
        try
        {
            var msgContent = retrievalResult?.Value?.Response?.FirstOrDefault()?.Content?.FirstOrDefault() as KnowledgeAgentMessageTextContent;
            sb.AppendLine(msgContent?.Text ?? string.Empty);
        }
        catch
        {
            sb.AppendLine(string.Empty);
        }

        // Activity
        sb.AppendLine();
        sb.AppendLine("Activity:");
        if (retrievalResult?.Value?.Activity != null)
        {
            foreach (var act in retrievalResult.Value.Activity)
            {
                sb.AppendLine($"Activity Type: {act.GetType().Name}");
                string activityJson = JsonSerializer.Serialize(
                    act,
                    act.GetType(),
                    new JsonSerializerOptions { WriteIndented = true }
                );
                sb.AppendLine(activityJson);
            }
        }

        // Results / References
        sb.AppendLine();
        sb.AppendLine("Results:");
        if (retrievalResult?.Value?.References != null)
        {
            foreach (var reference in retrievalResult.Value.References)
            {
                sb.AppendLine($"Reference Type: {reference.GetType().Name}");
                string referenceJson = JsonSerializer.Serialize(
                    reference,
                    reference.GetType(),
                    new JsonSerializerOptions { WriteIndented = true }
                );
                sb.AppendLine(referenceJson);
            }
        }

        var responseText = sb.ToString();

        // Return the consolidated Knowledge Agent's response
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

