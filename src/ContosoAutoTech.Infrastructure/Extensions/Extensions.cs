using ContosoAutoTech.Infrastructure.Azure.Speech;
using ContosoAutoTech.Infrastructure.Azure.DocumentIntelligence;
using ContosoAutoTech.Infrastructure.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AIAgent_AiAgentService = ContosoAutoTech.Infrastructure.AIAgent.AiAgentService;
using AiAgentService = ContosoAutoTech.Infrastructure.AIAgent.AiAgentService;
using AiInference_AiInferenceService = ContosoAutoTech.Infrastructure.AiInference.AiInferenceService;
using AiInferenceService = ContosoAutoTech.Infrastructure.AiInference.AiInferenceService;
using Speech_SpeechService = ContosoAutoTech.Infrastructure.Speech.SpeechService;
using SpeechService = ContosoAutoTech.Infrastructure.Speech.SpeechService;

namespace ContosoAutoTech.Infrastructure.Extensions;

public static class Extensions
{
    public static IServiceCollection AddAzure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        
        services.AddScoped<AIAgent_AiAgentService>();
        services.AddScoped<DocumentIntelligenceService>();
        services.AddScoped<AiInference_AiInferenceService>();
        services.AddScoped<Speech_SpeechService>();
        services.AddSingleton<EmailService>(x => new EmailService(configuration["Email:Secret"]!, configuration["Email:SenderEmail"]!));
        
        return services;
    }
}