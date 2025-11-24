using DiscountMcp.Models;
using DiscountMcp.Models.Requests;
using DiscountMcp.Models.Response;

namespace DiscountMcp;

public class DiscountWorkflowSession(string sessionId, DiscountRequest request)
{
    private readonly TaskCompletionSource<WorkflowStepResult> _initialResultTcs = new();
    private readonly TaskCompletionSource<bool> _approvalDecisionTcs = new();
    private readonly TaskCompletionSource<WorkflowStepResult> _finalResultTcs = new();

    public string SessionId { get; } = sessionId;
    public DiscountRequest Request { get; } = request;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public bool RequiresApproval { get; private set; }
    public bool IsCompleted { get; private set; }
    public string Status { get; private set; } = "CREATED";

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