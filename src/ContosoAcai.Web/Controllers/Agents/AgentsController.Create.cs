using ContosoAcai.Application.Agents;
using ContosoAcai.Application.Agents.Models.Requests;
using ContosoAcai.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAcai.Web.Controllers.Agents;

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