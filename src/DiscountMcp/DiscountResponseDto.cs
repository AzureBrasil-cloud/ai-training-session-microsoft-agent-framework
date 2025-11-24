namespace DiscountMcp;

public record DiscountResponseDto
{
    public string SessionId { get; init; } = "";
    public string Message { get; init; } = "";
    public bool RequiresApproval { get; init; }
    public bool Approved { get; init; }
    public decimal FinalPrice { get; init; }
    public string Status { get; init; } = "";
    public string? ApproverComments { get; init; }
}