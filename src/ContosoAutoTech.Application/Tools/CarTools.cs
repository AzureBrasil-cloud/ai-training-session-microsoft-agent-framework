using System.ComponentModel;
using System.Text.Json;
using ContosoAutoTech.Data;
using ContosoAutoTech.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;

namespace ContosoAutoTech.Application.Tools;

public class CarTools(AppDbContext context)
{
    [Description("Create a new car in the system.")]
    public async Task<string> CreateCar(
        [Description("The name/description of the car")] string name,
        [Description("The owner of the car")] string owner,
        [Description("The manufacturing year of the car")] int year,
        [Description("The model of the car")] string model,
        [Description("The price of the car in the local currency")] decimal price,
        [Description("The license plate of the car")] string licensePlate)
    {
        try
        {
            var car = new Car(name, owner, year, model, price, licensePlate);

            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();

            return JsonSerializer.Serialize(new
            {
                success = true,
                message = $"Car '{name}' created successfully!",
                carId = car.Id,
                details = new
                {
                    car.Id,
                    car.Name,
                    car.Owner,
                    car.Year,
                    car.Model,
                    car.Price,
                    car.LicensePlate,
                    car.CreatedAt
                }
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = $"Error creating car: {ex.Message}"
            });
        }
    }

    [Description("Update an existing car in the system.")]
    public async Task<string> UpdateCar(
        [Description("The ID of the car to update")] string carId,
        [Description("The new name/description of the car (optional)")] string? name = null,
        [Description("The new owner of the car (optional)")] string? owner = null,
        [Description("The new manufacturing year (optional)")] int? year = null,
        [Description("The new model (optional)")] string? model = null,
        [Description("The new price (optional)")] decimal? price = null,
        [Description("The new license plate (optional)")] string? licensePlate = null)
    {
        try
        {
            if (!Guid.TryParse(carId, out var id))
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Invalid car ID format"
                });
            }

            var car = await context.Cars.FindAsync(id);
            if (car == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = $"Car with ID '{carId}' not found"
                });
            }

            car.Update(name, owner, year, model, price, licensePlate);
            await context.SaveChangesAsync();

            return JsonSerializer.Serialize(new
            {
                success = true,
                message = $"Car '{car.Name}' updated successfully!",
                details = new
                {
                    car.Id,
                    car.Name,
                    car.Owner,
                    car.Year,
                    car.Model,
                    car.Price,
                    car.LicensePlate,
                    car.UpdatedAt
                }
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = $"Error updating car: {ex.Message}"
            });
        }
    }

    [Description("Delete a car from the system.")]
    public async Task<string> DeleteCar(
        [Description("The ID of the car to delete")] string carId)
    {
        try
        {
            if (!Guid.TryParse(carId, out var id))
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Invalid car ID format"
                });
            }

            var car = await context.Cars.FindAsync(id);
            if (car == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = $"Car with ID '{carId}' not found"
                });
            }

            var carName = car.Name;
            context.Cars.Remove(car);
            await context.SaveChangesAsync();

            return JsonSerializer.Serialize(new
            {
                success = true,
                message = $"Car '{carName}' deleted successfully!"
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = $"Error deleting car: {ex.Message}"
            });
        }
    }

    [Description("List cars from the system with optional filters.")]
    public async Task<string> ListCars(
        [Description("Filter by owner name (optional)")] string? owner = null,
        [Description("Filter by model (optional)")] string? model = null,
        [Description("Filter by minimum year (optional)")] int? minYear = null,
        [Description("Filter by maximum year (optional)")] int? maxYear = null,
        [Description("Filter by maximum price (optional)")] decimal? maxPrice = null,
        [Description("Filter by license plate (optional)")] string? licensePlate = null,
        [Description("Maximum number of results to return (default: 10)")] int limit = 10)
    {
        try
        {
            var query = context.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(owner))
                query = query.Where(c => c.Owner.Contains(owner));

            if (!string.IsNullOrWhiteSpace(model))
                query = query.Where(c => c.Model.Contains(model));

            if (minYear.HasValue)
                query = query.Where(c => c.Year >= minYear.Value);

            if (maxYear.HasValue)
                query = query.Where(c => c.Year <= maxYear.Value);

            if (maxPrice.HasValue)
                query = query.Where(c => c.Price <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(licensePlate))
                query = query.Where(c => c.LicensePlate != null && c.LicensePlate.Contains(licensePlate));

            var cars = await query
                .Take(limit)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return JsonSerializer.Serialize(new
            {
                success = true,
                count = cars.Count,
                cars = cars.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Owner,
                    c.Year,
                    c.Model,
                    c.Price,
                    c.LicensePlate,
                    c.CreatedAt,
                    c.UpdatedAt
                })
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = $"Error listing cars: {ex.Message}"
            });
        }
    }

    [Description("Get details of a specific car by ID.")]
    public async Task<string> GetCarById(
        [Description("The ID of the car to retrieve")] string carId)
    {
        try
        {
            if (!Guid.TryParse(carId, out var id))
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Invalid car ID format"
                });
            }

            var car = await context.Cars.FindAsync(id);
            if (car == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = $"Car with ID '{carId}' not found"
                });
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                car = new
                {
                    car.Id,
                    car.Name,
                    car.Owner,
                    car.Year,
                    car.Model,
                    car.Price,
                    car.LicensePlate,
                    car.CreatedAt,
                    car.UpdatedAt
                }
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = $"Error retrieving car: {ex.Message}"
            });
        }
    }
}

