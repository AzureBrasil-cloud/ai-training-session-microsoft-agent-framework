namespace DiscountMcp;

public record DiscountRequestDto
{
    public string ProductName { get; init; } = "";
    public decimal OriginalPrice { get; init; }
    public decimal RequestedDiscount { get; init; }
    public string CustomerCode { get; init; } = "";
    public string Reason { get; init; } = "";
}