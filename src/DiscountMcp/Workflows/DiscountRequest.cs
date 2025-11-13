namespace DiscountMcp;

/// <summary>
/// Dados da solicitação de desconto
/// </summary>
public record DiscountRequest
{
    public string ProductName { get; init; } = "";
    public decimal OriginalPrice { get; init; }
    public decimal RequestedDiscount { get; init; }
    public string CustomerName { get; init; } = "";
    public string Reason { get; init; } = "";
}