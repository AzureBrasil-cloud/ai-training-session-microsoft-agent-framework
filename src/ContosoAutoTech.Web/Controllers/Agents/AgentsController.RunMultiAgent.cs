using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Agents;

public partial class AgentsController
{
    [HttpPost("/api/agents/run-multi-agent")]
    public async Task<IActionResult> RunWorkflow(
        [FromBody] CreateMultiAgentsRunRequest request,
        [FromServices] AgentService service)
    {
        return (await service.RunMultiAgentsAsync(request)).ToApiResponse();
    }
}