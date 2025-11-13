using ContosoAutoTech.Application.CarSales;
using Microsoft.AspNetCore.Mvc;

namespace ContosoAutoTech.Web.Controllers.CarSales;

public partial class CarSalesController(CarSalesService carSalesService)
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var carSales = await carSalesService.GetAllAsync(cancellationToken);
        
        return Ok(carSales);
    }
}

