using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Agents;

public partial class AgentsController
{
    [HttpPost("/api/agents/run-workflow")]
    public async Task<IActionResult> RunWorkflow(
        [FromBody] CreateWorkflowRunRequest request,
        [FromServices] AgentService service)
    {
        return (await service.RunWorkflowAsync(request)).ToApiResponse();
    }
}