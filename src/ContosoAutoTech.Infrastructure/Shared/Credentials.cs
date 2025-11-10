namespace ContosoAutoTech.Infrastructure.Azure.Shared;

public record Credentials(
    string TenantId, 
    string ClientId, 
    string ClientSecret,
    string ProjectConnectionString);
