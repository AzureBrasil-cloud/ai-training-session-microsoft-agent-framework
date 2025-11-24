using DiscountMcp.Models.Requests;

namespace DiscountMcp.Workflows;

/// <summary>
/// Dados enviados para o RequestPort (inclui a solicitação original)
/// </summary>
internal record PendingApproval
{
    public DiscountRequest Request { get; init; } = null!;
    public ApprovalSignal Signal { get; init; }
}