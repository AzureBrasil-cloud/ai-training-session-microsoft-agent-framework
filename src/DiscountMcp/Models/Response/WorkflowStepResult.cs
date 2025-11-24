namespace DiscountMcp.Models.Response;

public record WorkflowStepResult
{
    public string Message { get; init; } = "";
    public bool RequiresApproval { get; init; }
    public bool Approved { get; init; }
    public decimal FinalPrice { get; init; }
}