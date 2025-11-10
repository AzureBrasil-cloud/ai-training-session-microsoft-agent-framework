using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Agents;

public partial class AgentsController
{
    [HttpPost("/api/agents/run")]
    public async Task<IActionResult> Run(
        [FromBody] CreateRunRequest request,
        [FromServices] AgentService service)
    {
        return (await service.RunAsync(request)).ToApiResponse();
    }
}