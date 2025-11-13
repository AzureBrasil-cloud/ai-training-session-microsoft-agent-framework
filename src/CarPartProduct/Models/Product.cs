namespace CarPartProduct.Models;

public class Product
{
    public Guid Id { get; set; }
    public string ProductCode { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
}