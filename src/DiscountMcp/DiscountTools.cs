using System.Collections.Concurrent;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.Agents.AI.Workflows;

namespace DiscountMcp;
[McpServerToolType]
public class DiscountTools(ILogger<DiscountTools> logger)
{
    private static readonly ConcurrentDictionary<string, DiscountWorkflowSession> _sessions = new();

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
        _sessions[sessionId] = session;

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
            Status = result.RequiresApproval ? "PENDING_APPROVAL" : (result.Approved ? "APPROVED" : "REJECTED")
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

        if (!_sessions.TryGetValue(sessionId, out var session))
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

    /// <summary>
    /// Lista todas as solicitações pendentes de aprovação
    /// </summary>
    [McpServerTool]
    [Description("List all pending discount approvals waiting for manager decision.")]
    public List<PendingApprovalDto> GetPendingApprovals()
    {
        logger.LogInformation("Fetching pending approvals");

        var pendingRequests = _sessions.Values
            .Where(s => s is { RequiresApproval: true, IsCompleted: false })
            .Select(s => new PendingApprovalDto
            {
                SessionId = s.SessionId,
                ProductName = s.Request.ProductName,
                OriginalPrice = s.Request.OriginalPrice,
                RequestedDiscount = s.Request.RequestedDiscount,
                CustomerCode = s.Request.CustomerCode,
                Reason = s.Request.Reason,
                RequestedAt = s.CreatedAt,
                DiscountAmount = s.Request.OriginalPrice * s.Request.RequestedDiscount,
                FinalPrice = s.Request.OriginalPrice * (1 - s.Request.RequestedDiscount)
            })
            .OrderByDescending(r => r.RequestedAt)
            .ToList();

        return pendingRequests;
    }

    /// <summary>
    /// Gerente aprova ou nega o desconto
    /// </summary>
    [McpServerTool]
    [Description("Approve or reject a pending discount request. Only managers can use this.")]
    public async Task<DiscountResponseDto> DecideApproval(
        [Description("Session ID of the request")] string sessionId,
        [Description("True to approve, false to reject")] bool approved,
        [Description("Optional comments from approver")] string? comments = null)
    {
        logger.LogInformation("Approval decision for session {SessionId}: {Decision}", sessionId, approved ? "APPROVED" : "REJECTED");

        if (!_sessions.TryGetValue(sessionId, out var session))
        {
            throw new InvalidOperationException($"Session {sessionId} not found");
        }

        if (!session.RequiresApproval)
        {
            throw new InvalidOperationException("This request does not require approval");
        }

        if (session.IsCompleted)
        {
            throw new InvalidOperationException("Request has already been processed");
        }

        // Envia a decisão para o workflow
        session.SendApprovalDecision(approved, comments);

        // Aguarda o resultado final
        var result = await session.WaitForFinalResultAsync();
        
        return new DiscountResponseDto
        {
            SessionId = sessionId,
            Message = result.Message,
            RequiresApproval = false,
            Approved = result.Approved,
            FinalPrice = result.FinalPrice,
            Status = result.Approved ? "APPROVED" : "REJECTED",
            ApproverComments = comments
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

// ========== DTOs ==========

public record DiscountRequestDto
{
    public string ProductName { get; init; } = "";
    public decimal OriginalPrice { get; init; }
    public decimal RequestedDiscount { get; init; }
    public string CustomerCode { get; init; } = "";
    public string Reason { get; init; } = "";
}

public record DiscountResponseDto
{
    public string SessionId { get; init; } = "";
    public string Message { get; init; } = "";
    public bool RequiresApproval { get; init; }
    public bool Approved { get; init; }
    public decimal FinalPrice { get; init; }
    public string Status { get; init; } = "";
    public string? ApproverComments { get; init; }
}

public record DiscountStatusDto
{
    public string SessionId { get; init; } = "";
    public string Status { get; init; } = "";
    public bool RequiresApproval { get; init; }
    public bool IsCompleted { get; init; }
    public DiscountRequestDto Request { get; init; } = null!;
}

public record PendingApprovalDto
{
    public string SessionId { get; init; } = "";
    public string ProductName { get; init; } = "";
    public decimal OriginalPrice { get; init; }
    public decimal RequestedDiscount { get; init; }
    public string CustomerCode { get; init; } = "";
    public string Reason { get; init; } = "";
    public DateTime RequestedAt { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal FinalPrice { get; init; }
}

public record ApprovalDecisionDto
{
    public string SessionId { get; init; } = "";
    public bool Approved { get; init; }
    public string? Comments { get; init; }
}

// ========== SESSION MANAGEMENT ==========

internal class DiscountWorkflowSession
{
    private readonly TaskCompletionSource<WorkflowStepResult> _initialResultTcs = new();
    private TaskCompletionSource<bool> _approvalDecisionTcs = new();
    private TaskCompletionSource<WorkflowStepResult> _finalResultTcs = new();

    public string SessionId { get; }
    public DiscountRequest Request { get; }
    public DateTime CreatedAt { get; }
    public bool RequiresApproval { get; private set; }
    public bool IsCompleted { get; private set; }
    public string Status { get; private set; } = "CREATED";

    public DiscountWorkflowSession(string sessionId, DiscountRequest request)
    {
        SessionId = sessionId;
        Request = request;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRequiringApproval()
    {
        RequiresApproval = true;
        Status = "PENDING_APPROVAL";
    }

    public void SetInitialResult(WorkflowStepResult result)
    {
        _initialResultTcs.TrySetResult(result);
        if (!result.RequiresApproval)
        {
            IsCompleted = true;
            Status = result.Approved ? "APPROVED" : "REJECTED";
        }
    }

    public void SetFinalResult(WorkflowStepResult result)
    {
        IsCompleted = true;
        Status = result.Approved ? "APPROVED" : "REJECTED";
        _finalResultTcs.TrySetResult(result);
    }

    public void SetError(string error)
    {
        IsCompleted = true;
        Status = "ERROR";
        var errorResult = new WorkflowStepResult
        {
            Message = error,
            RequiresApproval = false,
            Approved = false,
            FinalPrice = Request.OriginalPrice
        };
        _initialResultTcs.TrySetResult(errorResult);
        _finalResultTcs.TrySetResult(errorResult);
    }

    public async Task<WorkflowStepResult> WaitForResultAsync()
    {
        return await _initialResultTcs.Task;
    }

    public async Task<WorkflowStepResult> WaitForFinalResultAsync()
    {
        return await _finalResultTcs.Task;
    }

    public void SendApprovalDecision(bool approved, string? comments)
    {
        _approvalDecisionTcs.TrySetResult(approved);
    }

    public async Task<bool> WaitForApprovalDecisionAsync()
    {
        return await _approvalDecisionTcs.Task;
    }
}

internal record WorkflowStepResult
{
    public string Message { get; init; } = "";
    public bool RequiresApproval { get; init; }
    public bool Approved { get; init; }
    public decimal FinalPrice { get; init; }
}