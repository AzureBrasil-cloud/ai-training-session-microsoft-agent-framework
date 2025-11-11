namespace ContosoAutoTech.Infrastructure.AiSearch.Models;

public class AiSearchCredentials
{
    public string SearchEndpoint { get; set; } = string.Empty;
    public string SearchApiKey { get; set; } = string.Empty;
    public string IndexName { get; set; } = string.Empty;
    public string KnowledgeSourceName { get; set; } = string.Empty;
    public string KnowledgeAgentName { get; set; } = string.Empty;
    public string AoaiEndpoint { get; set; } = string.Empty;
    public string AoaiApiKey { get; set; } = string.Empty;
    public string AoaiDeployment { get; set; } = string.Empty;
    public string AoaiModel { get; set; } = string.Empty;
}

