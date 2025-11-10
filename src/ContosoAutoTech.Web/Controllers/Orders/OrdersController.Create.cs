using ContosoAutoTech.Application.Orders;
using ContosoAutoTech.Application.Orders.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Orders;

public partial class OrdersController
{
    [HttpPost("/api/orders")]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrdersRequest request,
        [FromServices] OrderService service)
    {
        return (await service.CreateAsync(request))
            .ToApiResponse();
    }
}