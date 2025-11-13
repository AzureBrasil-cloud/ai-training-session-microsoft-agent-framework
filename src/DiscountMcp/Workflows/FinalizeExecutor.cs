using Microsoft.Agents.AI.Workflows;

namespace DiscountMcp;

/// <summary>
/// Executor que finaliza o processo com base na decisão de aprovação
/// </summary>
internal sealed class FinalizeExecutor() : Executor<ApprovalDecision>("Finalize")
{
    public override async ValueTask HandleAsync(ApprovalDecision decision, IWorkflowContext context, CancellationToken cancellationToken = default)
    {
        var request = decision.Request;

        if (decision.Approved)
        {
            decimal finalPrice = request.OriginalPrice * (1 - request.RequestedDiscount);
            await context.YieldOutputAsync(
                new DiscountResult
                {
                    Approved = true,
                    FinalPrice = finalPrice,
                    Message = $"✅ Desconto aprovado pelo gerente!\n" +
                              $"Produto: {request.ProductName}\n" +
                              $"Desconto: {request.RequestedDiscount:P0}\n" +
                              $"Preço original: {request.OriginalPrice:C2}\n" +
                              $"Preço final: {finalPrice:C2}"
                },
                cancellationToken);
        }
        else
        {
            await context.YieldOutputAsync(
                new DiscountResult
                {
                    Approved = false,
                    FinalPrice = request.OriginalPrice,
                    Message = $"❌ Desconto negado pelo gerente.\n" +
                              $"Preço mantido: {request.OriginalPrice:C2}"
                },
                cancellationToken);
        }
    }
}