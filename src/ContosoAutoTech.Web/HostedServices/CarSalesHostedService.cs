using System.Diagnostics;
using ContosoAutoTech.Application;
using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Data.Entities;

namespace ContosoAutoTech.Web.HostedServices;

public class CarSalesHostedService(
    IServiceProvider serviceProvider,
    ILogger<CarSalesHostedService> logger,
    IConfiguration configuration) : IHostedService
{
    private static readonly ActivitySource ActivitySource = InstrumentationConfig.ActivitySource;
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var execute = configuration.GetValue<bool>("Application:ExecuteCarsSalesAgent");
        
        if(!execute) return;
        
        using var scope = serviceProvider.CreateScope();
        var agentService = scope.ServiceProvider.GetRequiredService<AgentService>();

        await ExecuteAsync(agentService);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    private async Task ExecuteAsync(AgentService agentService)
    {
        using Activity? activity = ActivitySource.StartActivity(nameof(CarSalesHostedService));
        
    }
}