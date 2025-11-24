namespace DiscountMcp;

public record DiscountStatusDto
{
    public string SessionId { get; init; } = "";
    public string Status { get; init; } = "";
    public bool RequiresApproval { get; init; }
    public bool IsCompleted { get; init; }
    public DiscountRequestDto Request { get; init; } = null!;
}