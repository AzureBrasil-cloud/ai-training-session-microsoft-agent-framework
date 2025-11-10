using ContosoAutoTech.Application.Reviews;
using ContosoAutoTech.Application.Reviews.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Reviews;

public partial class ReviewsController
{
    [HttpPost("/api/reviews")]
    public async Task<IActionResult> Create(
        [FromBody] CreateReviewRequest request,
        [FromServices] ReviewService service)
    {
        return (await service.CreateAsync(request)).ToApiResponse();
    }
}