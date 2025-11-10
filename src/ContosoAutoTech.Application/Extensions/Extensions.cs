using ContosoAutoTech.Data.Extensions;
using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Orders;
using ContosoAutoTech.Application.Reviews;
using ContosoAutoTech.Application.Threads;
using ContosoAutoTech.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoAutoTech.Application.Extensions;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Services
        services.AddScoped<ReviewService>();
        services.AddScoped<OrderService>();
        services.AddScoped<ThreadService>();
        services.AddScoped<AgentService>();
        
        // Domain
        services.AddData(configuration);
        
        // Infra
        services.AddAzure(configuration);
        
        return services;
    }
}