using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Application.Agents.Models.Results;
using PowerPilotChat.Application;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    public async Task<Result<AgentResult>> CreateAsync(CreateAgentRequest request)
    {
        throw new Exception();
    }
}