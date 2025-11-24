using DiscountMcp.Models.Requests;

namespace DiscountMcp.Models.Response;

public record DiscountStatusResponse
{
    public string SessionId { get; init; } = "";
    public string Status { get; init; } = "";
    public bool RequiresApproval { get; init; }
    public bool IsCompleted { get; init; }
    public DiscountRequest Request { get; init; } = null!;
}