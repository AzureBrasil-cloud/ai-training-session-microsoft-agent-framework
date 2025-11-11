using ContosoAutoTech.Infrastructure.AIAgent;
using ContosoAutoTech.Infrastructure.Email;
using ContosoAutoTech.Infrastructure.Mcps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoAutoTech.Infrastructure.Extensions;

public static class Extensions
{
    public static IServiceCollection AddAzure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        
        services.AddScoped<AiAgentService>();
        services.AddScoped<McpService>();
        services.AddSingleton<EmailService>(x => new EmailService(configuration["Email:Secret"]!, configuration["Email:SenderEmail"]!));
        
        return services;
    }
}