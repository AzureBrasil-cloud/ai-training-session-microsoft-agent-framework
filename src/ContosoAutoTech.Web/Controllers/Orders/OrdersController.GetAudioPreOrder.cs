using ContosoAutoTech.Application.Orders;
using ContosoAutoTech.Application.Orders.Models.Query;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Orders;

public partial class OrdersController
{
    [HttpGet("/api/pre-order/audio/{id}")]
    public async Task<IActionResult> GetAudioPreOrder(
        Guid id,
        bool applyAiTransformation,
        [FromServices] OrderService service)
    {
        return (await service.GetByIdAsync(new GetAudioPreOrderQuery(id, applyAiTransformation))).ToApiResponse();
    }
}