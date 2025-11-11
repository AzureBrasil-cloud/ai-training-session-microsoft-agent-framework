using ContosoAutoTech.Application.AiInference;
using ContosoAutoTech.Application.AiInference.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.AiInference;

public partial class AiInferenceController
{
    [HttpPost("/api/ai-inference/complete")]
    public async Task<IActionResult> Complete(
        [FromBody] ChatCompletionRequest request,
        [FromServices] AiInferenceApplicationService service)
    {
        return (await service.CompleteAsync(request)).ToApiResponse();
    }
}

