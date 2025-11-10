using ContosoAutoTech.Application.Orders;
using ContosoAutoTech.Application.Orders.Models.Query;
using ContosoAutoTech.Application.Orders.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Orders;

public partial class OrdersController
{
    [HttpGet("/api/pre-order/image")]
    public async Task<IActionResult> ListImagePreOrder([FromServices] OrderService service)
    {
        return (await service.ListImagePreOrdersAsync()).ToApiResponse();
    }
}