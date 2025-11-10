using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Agents;

public partial class AgentsController
{
    [HttpPost("/api/agents/thread")]
    public async Task<IActionResult> CreateThread([FromServices] AgentService service)
    {
        return (await service.CreateThreadAsync()).ToApiResponse();
    }
}