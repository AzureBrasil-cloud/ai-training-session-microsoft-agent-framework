using ContosoAutoTech.Data;
using Microsoft.Extensions.Configuration;
using AiAgentService = ContosoAutoTech.Infrastructure.AIAgent.AiAgentService;

namespace ContosoAutoTech.Application.Threads;

public partial class ThreadService(
    IConfiguration configuration, 
    AppDbContext context,
    AiAgentService aiAgentService);
