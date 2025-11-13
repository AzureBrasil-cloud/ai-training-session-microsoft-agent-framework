namespace CarPriceMcp.Models;

public class Prices
{
    public static IEnumerable<Price> Items => new List<Price>()
    {
        new() { Id = Guid.NewGuid(), ProductCode = "WH001", Value = 0.00m },
        new() { Id = Guid.NewGuid(), ProductCode = "CM002", Value = 12.00m },
        new() { Id = Guid.NewGuid(), ProductCode = "BS003", Value = 76.00m },
        new() { Id = Guid.NewGuid(), ProductCode = "NS004", Value = 50.00m },
        new() { Id = Guid.NewGuid(), ProductCode = "WB005", Value = 10.00m },
        new() { Id = Guid.NewGuid(), ProductCode = "DL006", Value = 66.00m },
        new() { Id = Guid.NewGuid(), ProductCode = "PC007", Value = 24.00m}
    };
}