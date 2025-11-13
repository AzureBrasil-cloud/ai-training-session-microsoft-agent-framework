using ContosoAutoTech.Application.CarSales.Models.Results;
using ContosoAutoTech.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoAutoTech.Application.CarSales;

public partial class CarSalesService
{
    public async Task<List<CarSaleResult>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var carSales = await context.CarSales
            .OrderByDescending(cs => cs.CreatedAt)
            .ToListAsync(cancellationToken);

        return carSales.Select(cs => new CarSaleResult(
            cs.Id,
            cs.ImageUrl,
            cs.Model,
            cs.LicensePlate,
            cs.Color,
            cs.Price,
            cs.Description,
            cs.CarFeatures.Strengths,
            cs.CarFeatures.Weaknesses,
            cs.CreatedAt
        )).ToList();
    }
}

