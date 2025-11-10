using ContosoAutoTech.Application.Threads;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Threads;

public partial class ThreadsController
{
    [HttpPost("/api/threads")]
    public async Task<IActionResult> Create([FromServices] ThreadService service)
    {
        return (await service.CreateAsync()).ToApiResponse();
    }
}