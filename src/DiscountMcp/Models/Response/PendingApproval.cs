namespace DiscountMcp.Models.Response;

public record PendingApproval
{
    public string SessionId { get; init; } = "";
    public string ProductName { get; init; } = "";
    public decimal OriginalPrice { get; init; }
    public decimal RequestedDiscount { get; init; }
    public string CustomerCode { get; init; } = "";
    public string Reason { get; init; } = "";
    public DateTime RequestedAt { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal FinalPrice { get; init; }
}