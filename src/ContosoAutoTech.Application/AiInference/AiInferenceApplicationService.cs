using ContosoAutoTech.Infrastructure.AiInference;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContosoAutoTech.Application.AiInference;

public partial class AiInferenceApplicationService(
    ILogger<AiInferenceApplicationService> logger, 
    IConfiguration configuration,
    AiInferenceService aiInferenceService)
{
    private Credentials GetCredentials()
    {
        var foundryEndpoint = configuration["AiFoundry:Endpoint"]!;
        var foudryApiKey = configuration["AiFoundry:ApiKey"]!;
        var model = configuration["AiFoundry:ChatModel"]!;

        return new Credentials(foundryEndpoint, foudryApiKey, model);
    }
}

