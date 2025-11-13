using Microsoft.Agents.AI.Workflows;

namespace DiscountMcp;

public static class WorkflowFactory
{
    /// <summary>
    /// Cria um workflow para solicitação de desconto com aprovação
    /// </summary>
    public static Workflow BuildWorkflow()
    {
        // RequestPort que recebe PendingApproval e retorna ApprovalDecision
        RequestPort approvalRequestPort = RequestPort.Create<PendingApproval, ApprovalDecision>("ApproveDiscount");
        
        // Executores
        ValidationExecutor validationExecutor = new();
        FinalizeExecutor finalizeExecutor = new();

        // Constrói o workflow
        return new WorkflowBuilder(validationExecutor)
            .AddEdge(validationExecutor, approvalRequestPort)
            .AddEdge(approvalRequestPort, finalizeExecutor)
            .WithOutputFrom(validationExecutor)
            .WithOutputFrom(finalizeExecutor)
            .Build();
    }
}