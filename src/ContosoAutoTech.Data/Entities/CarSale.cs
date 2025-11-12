using ContosoAutoTech.Data.Entities.Interfaces;

namespace ContosoAutoTech.Data.Entities;

public class CarSale : IEntity
{
    public Guid Id { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    public List<string> Strengths { get; set; } = null!;
    public List<string> Weaknesses { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    // EF
    private CarSale() { }

    public CarSale(
        string model,
        string licensePlate,
        string color,
        decimal price,
        string description,
        List<string> strengths,
        List<string> weaknesses)
    {
        Id = Guid.NewGuid();
        Model = model;
        LicensePlate = licensePlate;
        Color = color;
        Price = price;
        Description = description;
        Strengths = strengths;
        Weaknesses = weaknesses;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string? model = null,
        string? licensePlate = null,
        string? color = null,
        decimal? price = null,
        string? description = null,
        List<string>? strengths = null,
        List<string>? weaknesses = null)
    {
        if (model != null) Model = model;
        if (licensePlate != null) LicensePlate = licensePlate;
        if (color != null) Color = color;
        if (price.HasValue) Price = price.Value;
        if (description != null) Description = description;
        if (strengths != null) Strengths = strengths;
        if (weaknesses != null) Weaknesses = weaknesses;
    }
}

