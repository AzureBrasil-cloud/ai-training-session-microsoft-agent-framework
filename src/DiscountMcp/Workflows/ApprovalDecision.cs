using DiscountMcp.Models.Requests;

namespace DiscountMcp.Workflows;

/// <summary>
/// Decisão de aprovação com os dados originais da solicitação
/// </summary>
public record ApprovalDecision
{
    public DiscountRequest Request { get; init; } = null!;
    public bool Approved { get; init; }
}