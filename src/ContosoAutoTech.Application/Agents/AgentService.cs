using ContosoAutoTech.Data;
using ContosoAutoTech.Infrastructure.AIAgent;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService(
    AppDbContext context,
    IConfiguration configuration,
    ILogger<AgentService> logger,
    AiAgentService aiAgentService)
{
    private Credentials GetCredentials()
    {
        var foundryEndpoint = configuration["AiFoundry:Endpoint"]!;
        var foudryApiKey = configuration["AiFoundry:ApiKey"]!;
        var model = configuration["AiFoundry:ChatModel"]!;

        return new Credentials(foundryEndpoint, foudryApiKey, model);
    }
};
