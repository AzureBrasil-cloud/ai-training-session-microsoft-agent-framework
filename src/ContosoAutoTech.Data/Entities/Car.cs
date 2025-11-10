using ContosoAutoTech.Data.Entities.Interfaces;

namespace ContosoAutoTech.Data.Entities;

public class Car : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Owner { get; set; } = null!;
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public decimal Price { get; set; }
    public string? LicensePlate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // EF
    private Car() { }

    public Car(
        string name, 
        string owner,
        int year,
        string model,
        decimal price,
        string licensePlate)
    {
        Id = Guid.NewGuid();
        Name = name;
        Owner = owner;
        Year = year;
        Model = model;
        Price = price;
        LicensePlate = licensePlate;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string? name = null, 
        string? owner = null, 
        int? year = null, 
        string? model = null, 
        decimal? price = null, 
        string? licensePlate = null)
    {
        if (name != null) Name = name;
        if (owner != null) Owner = owner;
        if (year.HasValue) Year = year.Value;
        if (model != null) Model = model;
        if (price.HasValue) Price = price.Value;
        if (licensePlate != null) LicensePlate = licensePlate;
        
        UpdatedAt = DateTime.UtcNow;
    }
}

