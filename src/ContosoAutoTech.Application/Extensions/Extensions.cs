using ContosoAutoTech.Data.Extensions;
using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Tools;
using ContosoAutoTech.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoAutoTech.Application.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Services
        services.AddScoped<AgentService>();
        
        // // Tools
        // services.AddScoped<CarTools>();
        
        // Domain
        services.AddData(configuration);
        
        // Infra
        services.AddAzure(configuration);
        
        return services;
    }
}