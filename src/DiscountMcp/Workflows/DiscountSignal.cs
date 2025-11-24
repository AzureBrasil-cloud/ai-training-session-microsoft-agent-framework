namespace DiscountMcp.Workflows;

/// <summary>
/// Sinais para controle do fluxo de desconto
/// </summary>
public enum DiscountSignal
{
    Init,
    ValidationRequired,
    Validated,
    Rejected
}