using Microsoft.Agents.AI.Workflows;

namespace DiscountMcp;

/// <summary>
/// Executor que valida a solicitação de desconto
/// </summary>
internal sealed class ValidationExecutor() : Executor<DiscountRequest>("Validation")
{
    private const decimal MaxAutoApprovalDiscount = 0.10m; // 10%

    public override async ValueTask HandleAsync(DiscountRequest request, IWorkflowContext context, CancellationToken cancellationToken = default)
    {
        // Validações básicas
        if (request.RequestedDiscount <= 0 || request.RequestedDiscount >= 1)
        {
            await context.YieldOutputAsync(
                new DiscountResult
                {
                    Approved = false,
                    FinalPrice = request.OriginalPrice,
                    Message = "❌ Desconto inválido. Deve ser entre 0% e 100%."
                },
                cancellationToken);
            return;
        }

        if (string.IsNullOrWhiteSpace(request.ProductName))
        {
            await context.YieldOutputAsync(
                new DiscountResult
                {
                    Approved = false,
                    FinalPrice = request.OriginalPrice,
                    Message = "❌ Nome do produto é obrigatório."
                },
                cancellationToken);
            return;
        }

        // Se o desconto for pequeno, aprova automaticamente
        if (request.RequestedDiscount <= MaxAutoApprovalDiscount)
        {
            decimal finalPrice = request.OriginalPrice * (1 - request.RequestedDiscount);
            await context.YieldOutputAsync(
                new DiscountResult
                {
                    Approved = true,
                    FinalPrice = finalPrice,
                    Message = $"✅ Desconto de {request.RequestedDiscount:P0} aprovado automaticamente! Preço final: {finalPrice:C2}"
                },
                cancellationToken);
            return;
        }

        // Desconto requer aprovação - envia a solicitação para o RequestPort
        // O RequestPort vai enviar ApprovalSignal e aguardar resposta
        await context.SendMessageAsync(
            new PendingApproval { Request = request, Signal = ApprovalSignal.PendingApproval },
            cancellationToken: cancellationToken);
    }
}