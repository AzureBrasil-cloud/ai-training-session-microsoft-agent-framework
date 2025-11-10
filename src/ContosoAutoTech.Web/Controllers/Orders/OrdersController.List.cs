using ContosoAutoTech.Application.Orders;
using ContosoAutoTech.Application.Orders.Models.Query;
using ContosoAutoTech.Application.Orders.Models.Requests;
using ContosoAutoTech.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.Orders;

public partial class OrdersController
{
    [HttpGet("/api/orders")]
    public async Task<IActionResult> Create(
        string userEmail,
        [FromServices] OrderService service)
    {
        return (await service.ListByEmail(new ListByEmailQuery(userEmail)))
            .ToApiResponse();
    }
}