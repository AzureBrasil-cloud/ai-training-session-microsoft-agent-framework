using ContosoAutoTech.Application.Orders;
using ContosoAutoTech.Application.Orders.Models.Query;
using ContosoAutoTech.Application.Orders.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Orders;

public partial class OrdersController
{
    [HttpGet("/api/pre-order/audio")]
    public async Task<IActionResult> ListAudioPreOrder([FromServices] OrderService service)
    {
        return (await service.ListAudioPreOrder()).ToApiResponse();
    }
}