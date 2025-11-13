using ContosoAutoTech.Data.Extensions;
using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.AiInference;
using ContosoAutoTech.Application.CarSales;
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
        services.AddScoped<AiInferenceApplicationService>();
        services.AddScoped<CarSalesService>();
        
        // Domain
        services.AddData(configuration);
        
        // Infra
        services.AddAzure(configuration);
        
        return services;
    }
}