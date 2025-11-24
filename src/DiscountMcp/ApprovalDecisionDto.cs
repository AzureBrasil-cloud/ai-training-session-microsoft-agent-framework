namespace DiscountMcp;

public record ApprovalDecisionDto
{
    public string SessionId { get; init; } = "";
    public bool Approved { get; init; }
    public string? Comments { get; init; }
}