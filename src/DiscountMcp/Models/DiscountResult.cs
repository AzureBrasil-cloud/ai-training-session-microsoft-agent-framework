namespace DiscountMcp;

/// <summary>
/// Resultado do workflow
/// </summary>
public record DiscountResult
{
    public bool Approved { get; init; }
    public decimal FinalPrice { get; init; }
    public string Message { get; init; } = "";
    public string? ApproverComments { get; init; }
}