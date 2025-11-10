using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Agents;

public partial class AgentsController
{
    [HttpPost("/api/agents")]
    public async Task<IActionResult> Create(
        [FromBody] CreateAgentRequest request,
        [FromServices] AgentService service)
    {
        return (await service.CreateAsync(request)).ToApiResponse();
    }
}