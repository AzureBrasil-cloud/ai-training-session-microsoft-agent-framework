using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using ContosoAutoTech.Data;
using ContosoAutoTech.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContosoAutoTech.Application.Tools;

public class CarTools(AppDbContext context)
{
    private static readonly ActivitySource ActivitySource = InstrumentationConfig.ActivitySource;

    [Description("Create a new car in the system.")]
    public async Task<string> CreateCar(
        [Description("The name/description of the car")] string name,
        [Description("The owner of the car")] string owner,
        [Description("The manufacturing year of the car")] int year,
        [Description("The model of the car")] string model,
        [Description("The price of the car in the local currency")] decimal price,
        [Description("The license plate of the car")] string licensePlate)
    {
        using Activity? activity = ActivitySource.StartActivity();
        activity?.SetTag("car.operation", "create");
        activity?.SetTag("car.name", name);
        activity?.SetTag("car.owner", owner);
        activity?.SetTag("car.year", year);
        activity?.SetTag("car.model", model);
        activity?.SetTag("car.price", price);
        activity?.SetTag("car.license_plate", licensePlate);
        
        try
        {
            var car = new Car(name, owner, year, model, price, licensePlate);

            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();

            activity?.SetTag("car.id", car.Id);
            activity?.SetTag("car.created_at", car.CreatedAt);
            activity?.SetTag("operation.success", true);

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
            activity?.SetTag("operation.success", false);
            activity?.SetTag("error.message", ex.Message);
            activity?.SetTag("error.type", ex.GetType().Name);
            
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
        using Activity? activity = ActivitySource.StartActivity();
        activity?.SetTag("car.operation", "update");
        activity?.SetTag("car.id", carId);
        if (name != null) activity?.SetTag("car.new_name", name);
        if (owner != null) activity?.SetTag("car.new_owner", owner);
        if (year != null) activity?.SetTag("car.new_year", year);
        if (model != null) activity?.SetTag("car.new_model", model);
        if (price != null) activity?.SetTag("car.new_price", price);
        if (licensePlate != null) activity?.SetTag("car.new_license_plate", licensePlate);
        
        try
        {
            if (!Guid.TryParse(carId, out var id))
            {
                activity?.SetTag("operation.success", false);
                activity?.SetTag("error.reason", "invalid_id_format");
                
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Invalid car ID format"
                });
            }

            var car = await context.Cars.FindAsync(id);
            if (car == null)
            {
                activity?.SetTag("operation.success", false);
                activity?.SetTag("error.reason", "car_not_found");
                
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = $"Car with ID '{carId}' not found"
                });
            }

            car.Update(name, owner, year, model, price, licensePlate);
            await context.SaveChangesAsync();

            activity?.SetTag("car.name", car.Name);
            activity?.SetTag("car.updated_at", car.UpdatedAt);
            activity?.SetTag("operation.success", true);

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
            activity?.SetTag("operation.success", false);
            activity?.SetTag("error.message", ex.Message);
            activity?.SetTag("error.type", ex.GetType().Name);
            
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
        using Activity? activity = ActivitySource.StartActivity();
        activity?.SetTag("car.operation", "delete");
        activity?.SetTag("car.id", carId);
        
        try
        {
            if (!Guid.TryParse(carId, out var id))
            {
                activity?.SetTag("operation.success", false);
                activity?.SetTag("error.reason", "invalid_id_format");
                
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Invalid car ID format"
                });
            }

            var car = await context.Cars.FindAsync(id);
            if (car == null)
            {
                activity?.SetTag("operation.success", false);
                activity?.SetTag("error.reason", "car_not_found");
                
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = $"Car with ID '{carId}' not found"
                });
            }

            var carName = car.Name;
            activity?.SetTag("car.name", carName);
            
            context.Cars.Remove(car);
            await context.SaveChangesAsync();

            activity?.SetTag("operation.success", true);

            return JsonSerializer.Serialize(new
            {
                success = true,
                message = $"Car '{carName}' deleted successfully!"
            });
        }
        catch (Exception ex)
        {
            activity?.SetTag("operation.success", false);
            activity?.SetTag("error.message", ex.Message);
            activity?.SetTag("error.type", ex.GetType().Name);
            
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
        using Activity? activity = ActivitySource.StartActivity();
        activity?.SetTag("car.operation", "list");
        if (owner != null) activity?.SetTag("filter.owner", owner);
        if (model != null) activity?.SetTag("filter.model", model);
        if (minYear != null) activity?.SetTag("filter.min_year", minYear);
        if (maxYear != null) activity?.SetTag("filter.max_year", maxYear);
        if (maxPrice != null) activity?.SetTag("filter.max_price", maxPrice);
        if (licensePlate != null) activity?.SetTag("filter.license_plate", licensePlate);
        
        activity?.SetTag("filter.limit", limit);
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

            activity?.SetTag("result.count", cars.Count);
            activity?.SetTag("operation.success", true);

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
            activity?.SetTag("operation.success", false);
            activity?.SetTag("error.message", ex.Message);
            activity?.SetTag("error.type", ex.GetType().Name);
           
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
        using Activity? activity = ActivitySource.StartActivity();
        activity?.SetTag("car.operation", "get_by_id");
        activity?.SetTag("car.id", carId);
        
        try
        {
            if (!Guid.TryParse(carId, out var id))
            {
                activity?.SetTag("operation.success", false);
                activity?.SetTag("error.reason", "invalid_id_format");
                
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Invalid car ID format"
                });
            }

            var car = await context.Cars.FindAsync(id);
            if (car == null)
            {
                activity?.SetTag("operation.success", false);
                activity?.SetTag("error.reason", "car_not_found");
                
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    message = $"Car with ID '{carId}' not found"
                });
            }

            activity?.SetTag("car.name", car.Name);
            activity?.SetTag("car.owner", car.Owner);
            activity?.SetTag("car.year", car.Year);
            activity?.SetTag("car.model", car.Model);
            activity?.SetTag("car.price", car.Price);
            activity?.SetTag("operation.success", true);

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
            activity?.SetTag("operation.success", false);
            activity?.SetTag("error.message", ex.Message);
            activity?.SetTag("error.type", ex.GetType().Name);
            
            return JsonSerializer.Serialize(new
            {
                success = false,
                message = $"Error retrieving car: {ex.Message}"
            });
        }
    }
}

