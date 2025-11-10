using ContosoAutoTech.Application.Agents;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Agents;

public partial class AgentsController
{
    [HttpGet("/api/agents/threads/{threadId}/messages")]
    public async Task<IActionResult> GetThreadMessages(
        [FromRoute] Guid threadId,
        [FromServices] AgentService service)
    {
        return (await service.GetThreadMessagesAsync(threadId)).ToApiResponse();
    }
}

