using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Agents;

public partial class AgentsController
{
    [HttpGet("/api/agents/threads")]
    public async Task<IActionResult> ListThreadsByFeature(
        [FromQuery] ListThreadsByFeatureRequest request,
        [FromServices] AgentService service)
    {
        return (await service.ListThreadsByFeatureAsync(request)).ToApiResponse();
    }
}

