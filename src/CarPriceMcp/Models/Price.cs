namespace CarPriceMcp.Models;

public class Price
{
    public Guid Id { get; set; }
    public string ProductCode { get; set; }
    public decimal Value { get; set; }
}