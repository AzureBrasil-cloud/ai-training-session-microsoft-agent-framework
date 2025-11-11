using System.Diagnostics;
using Azure.Monitor.OpenTelemetry.Exporter;
using ContosoAutoTech.Application;
using ContosoAutoTech.Application.Extensions;
using PowerPilotChat.Web.Middlewares;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

namespace ContosoAutoTech.Web.Extensions;

public static class ConfigurationExtensions
{
    private const string ServiceName = "ContosoAutoTech.Api";
    
    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(ServiceName)); 
            options.AddOtlpExporter();
            options.IncludeFormattedMessage = true;
            options.ParseStateValues = true;
            options.IncludeScopes = true;
        });
    }
    
    public static void AddConfigurations(
        this IServiceCollection services, 
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var mvcBuilder = services.AddControllersWithViews();
        
        if (environment.IsDevelopment())
        {
            mvcBuilder.AddRazorRuntimeCompilation();
        }
        
        services.AddHttpContextAccessor();
        
        // Global exception handler
        services.AddTransient<GlobalExceptionHandlerMiddleware>();
        
        AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true); 
        AppContext.SetSwitch("Azure.Experimental.TraceGenAIMessageContent", true);
        AppContext.SetSwitch("System.Diagnostics.DiagnosticSource.EFCoreInstrumentation", true);

        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .ConfigureResource(resource => resource.AddService(ServiceName))    
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSource(InstrumentationConfig.ActivitySource.Name)
                .AddSource("*Microsoft.Extensions.AI") 
                .AddSource("*Microsoft.Extensions.Agents*")
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .ConfigureResource(resource => resource.AddService(ServiceName))    
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddMeter("*Microsoft.Agents.AI")
                .AddOtlpExporter());
        
        // Application
        services.AddApplication(configuration);
    }

    public static void ConfigureApplication(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseDefaultFiles();
        app.MapStaticAssets();
        
        app.MapControllers();
        
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}")
            .WithStaticAssets();
        
        app.MapFallbackToFile(app.Environment.IsDevelopment() ? "/index_dev.html" : "/client/index.html");
    }
}
