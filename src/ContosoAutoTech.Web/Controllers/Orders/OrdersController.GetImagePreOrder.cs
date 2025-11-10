using ContosoAutoTech.Application.Orders;
using ContosoAutoTech.Application.Orders.Models.Query;
using ContosoAutoTech.Application.Orders.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Orders;

public partial class OrdersController
{
    [HttpGet("/api/pre-order/image/{id}")]
    public async Task<IActionResult> GetImagePreOrder(
        Guid id,
        bool applyAiTransformation,
        [FromServices] OrderService service)
    {
        return (await service.GetByIdAsync(new GetImagePreOrderQuery(id, applyAiTransformation))).ToApiResponse();
    }
}