namespace ContosoAutoTech.Infrastructure.Shared;

public record Credentials(
    string FoundryEndpoint, 
    string FoundryApiKey,
    string ModelDeploymentName);
