using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.Agents.AI.Workflows;

namespace DiscountMcp;

[McpServerToolType]
public class DiscountTools(ILogger<DiscountTools> logger, DiscountSessionStore store)
{
    /// <summary>
    /// Cliente inicia solicitação de desconto
    /// </summary>
    [McpServerTool]
    [Description("Request a discount for a product. Returns session ID and approval status.")]
    public async Task<DiscountResponseDto> RequestDiscount(
        [Description("Product name")] string productName,
        [Description("Original price")] decimal originalPrice,
        [Description("Requested discount (0.0 to 1.0)")] decimal requestedDiscount,
        [Description("Customer code")] string customerCode,
        [Description("Reason for discount request")] string reason)
    {
        logger.LogInformation("Discount request received for {ProductName}", productName);

        string sessionId = Guid.NewGuid().ToString();

        var discountRequest = new DiscountRequest
        {
            ProductName = productName,
            OriginalPrice = originalPrice,
            RequestedDiscount = requestedDiscount,
            CustomerCode = customerCode,
            Reason = reason
        };

        var session = new DiscountWorkflowSession(sessionId, discountRequest);

        // 🔥 Agora usando o Singleton store compartilhado
        store.Add(sessionId, session);

        // Inicia o workflow em background
        _ = Task.Run(async () => await RunWorkflowAsync(session));

        // Aguarda o resultado inicial
        var result = await session.WaitForResultAsync();

        return new DiscountResponseDto
        {
            SessionId = sessionId,
            Message = result.Message,
            RequiresApproval = result.RequiresApproval,
            Approved = result.Approved,
            FinalPrice = result.FinalPrice,
            Status = result.RequiresApproval
                ? "PENDING_APPROVAL"
                : (result.Approved ? "APPROVED" : "REJECTED")
        };
    }


    /// <summary>
    /// Cliente consulta status da solicitação
    /// </summary>
    [McpServerTool]
    [Description("Check the status of a discount request by session ID.")]
    public DiscountStatusDto GetDiscountStatus(
        [Description("Session ID from discount request")] string sessionId)
    {
        logger.LogInformation("Status check for session {SessionId}", sessionId);
        
        if (!store.TryGet(sessionId, out var session))
        {
            throw new InvalidOperationException($"Session {sessionId} not found");
        }

        return new DiscountStatusDto
        {
            SessionId = sessionId,
            Status = session.Status,
            RequiresApproval = session.RequiresApproval,
            IsCompleted = session.IsCompleted,
            Request = new DiscountRequestDto
            {
                ProductName = session.Request.ProductName,
                OriginalPrice = session.Request.OriginalPrice,
                RequestedDiscount = session.Request.RequestedDiscount,
                CustomerCode = session.Request.CustomerCode,
                Reason = session.Request.Reason
            }
        };
    }
    
    // ========== WORKFLOW EXECUTION ==========

    private async Task RunWorkflowAsync(DiscountWorkflowSession session)
    {
        try
        {
            var workflow = WorkflowFactory.BuildWorkflow();
            
            // Inicia o workflow passando a solicitação inicial
            await using StreamingRun handle = await InProcessExecution.StreamAsync(
                workflow,
                session.Request);
            
            await foreach (WorkflowEvent evt in handle.WatchStreamAsync())
            {
                switch (evt)
                {
                    case RequestInfoEvent requestInputEvt:
                        var pendingApproval = requestInputEvt.Request.DataAs<PendingApproval>();
                        
                        if (pendingApproval?.Signal == ApprovalSignal.PendingApproval)
                        {
                            session.MarkAsRequiringApproval();
                            
                            session.SetInitialResult(new WorkflowStepResult
                            {
                                Message = $"⏳ Aguardando aprovação do gerente para desconto de {session.Request.RequestedDiscount:P0}",
                                RequiresApproval = true,
                                Approved = false,
                                FinalPrice = session.Request.OriginalPrice
                            });

                            bool approved = await session.WaitForApprovalDecisionAsync();
                            
                            var approvalDecision = new ApprovalDecision
                            {
                                Request = pendingApproval.Request,
                                Approved = approved
                            };
                            
                            ExternalResponse response = requestInputEvt.Request.CreateResponse(approvalDecision);
                            await handle.SendResponseAsync(response);
                        }
                        break;

                    case WorkflowOutputEvent outputEvt:
                        var discountResult = outputEvt.Data as DiscountResult;
                        
                        if (discountResult != null)
                        {
                            if (session.RequiresApproval)
                            {
                                session.SetFinalResult(new WorkflowStepResult
                                {
                                    Message = discountResult.Message,
                                    RequiresApproval = false,
                                    Approved = discountResult.Approved,
                                    FinalPrice = discountResult.FinalPrice
                                });
                            }
                            else
                            {
                                session.SetInitialResult(new WorkflowStepResult
                                {
                                    Message = discountResult.Message,
                                    RequiresApproval = false,
                                    Approved = discountResult.Approved,
                                    FinalPrice = discountResult.FinalPrice
                                });
                            }
                        }
                        return;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Workflow error for session {SessionId}", session.SessionId);
            session.SetError($"Erro no workflow: {ex.Message}");
        }
    }
}