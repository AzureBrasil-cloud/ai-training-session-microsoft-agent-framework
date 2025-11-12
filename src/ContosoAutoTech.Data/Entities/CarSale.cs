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
    public Features CarFeatures { get; set; } = null!;
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
        CarFeatures = new Features
        {
            Strengths = strengths,
            Weaknesses = weaknesses
        };
        CreatedAt = DateTime.UtcNow;
    }

    public class Features
    {
        public List<string> Strengths { get; set; } = null!;
        public List<string> Weaknesses { get; set; } = null!;
    }
}

