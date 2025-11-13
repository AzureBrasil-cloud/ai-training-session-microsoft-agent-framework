using ContosoAutoTech.Data.Entities.Interfaces;

namespace ContosoAutoTech.Data.Entities;

public class CarReferencePrice : IEntity
{
    public Guid Id { get; set; }
    public string Model { get; set; } = null!;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }

    // EF
    private CarReferencePrice() { }

    public CarReferencePrice(string model, decimal price)
    {
        Id = Guid.NewGuid();
        Model = model;
        Price = price;
        CreatedAt = DateTime.UtcNow;
    }
}

