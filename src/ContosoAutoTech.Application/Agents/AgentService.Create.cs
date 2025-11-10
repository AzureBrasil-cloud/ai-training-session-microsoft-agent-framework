using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Application.Agents.Models.Results;
using ContosoAutoTech.Infrastructure.Shared;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    public async Task<Result<AgentResult>> CreateAsync(CreateAgentRequest request)
    {
        var foundryEndpoint = configuration["AiFoundry:Endpoint"]!;
        var foudryApiKey = configuration["AiFoundry:ApiKey"]!;
        var model = configuration["AiFoundry:ChatModel"]!;
        
        var credentials = new Credentials(foundryEndpoint, foudryApiKey, model);
       
        var thread = aiAgentService.CreateThread(
            credentials,
            "teste",
            "You are a helpful assistant.");
        
       var agent = aiAgentService.CreateAgent(
            credentials,
            "teste",
            "You are a helpful assistant.");

       var result1 = await agent.RunAsync("tell me a joke in pt-br", thread);
       var result2 = await agent.RunAsync("now the same in en-us", thread);
       
        throw new Exception();
    }
}