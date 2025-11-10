using ContosoAutoTech.Data;
using ContosoAutoTech.Infrastructure.AIAgent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService(
    AppDbContext context,
    IConfiguration configuration,
    ILogger<AgentService> logger,
    AiAgentService aiAgentService);
