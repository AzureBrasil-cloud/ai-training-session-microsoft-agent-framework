using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Application.Agents.Models.Results;
using ContosoAutoTech.Infrastructure.Shared;
using PowerPilotChat.Application;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    public async Task<Result<AgentResult>> CreateAsync(CreateAgentRequest request)
    {
        var foundryEndpoint = configuration["AiFoundry:Endpoint"]!;
        var foudryApiKey = configuration["AiFoundry:ApiKey"]!;
        var model = configuration["AiFoundry:ChatModel"]!;
        
        var credentials = new Credentials(foundryEndpoint, foudryApiKey, model);
        
        await aiAgentService.CreateAgent(
            credentials,
            "teste",
            "You are a helpful assistant.");

        throw new Exception();
    }
}