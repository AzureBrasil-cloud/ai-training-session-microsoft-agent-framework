using System.ComponentModel;
using DiscountMcp.Models;
using DiscountMcp.Models.Response;
using DiscountMcp.Tools;
using ModelContextProtocol.Server;

namespace DiscountMcp;

[McpServerToolType]
public class ManagerDiscountTools(ILogger<DiscountTools> logger, DiscountSessionStore store)
{
    /// <summary>
    /// Lista todas as solicitações pendentes de aprovação
    /// </summary>
    [McpServerTool]
    [Description("List all pending discount approvals waiting for manager decision.")]
    public List<PendingApprovalResponse> GetPendingApprovals()
    {
        logger.LogInformation("Fetching pending approvals");

        var pendingRequests = store
            .GetPendingApprovals()   // Agora vem direto da Store
            .Select(s => new PendingApprovalResponse
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
    public async Task<DiscountResponse> DecideApproval(
        [Description("Session ID of the request")] string sessionId,
        [Description("True to approve, false to reject")] bool approved,
        [Description("Optional comments from approver")] string? comments = null)
    {
        logger.LogInformation("Approval decision for session {SessionId}: {Decision}",
            sessionId, approved ? "APPROVED" : "REJECTED");

        // Usa o store ao invés do static
        if (!store.TryGet(sessionId, out var session))
            throw new InvalidOperationException($"Session {sessionId} not found");

        if (!session.RequiresApproval)
            throw new InvalidOperationException("This request does not require approval");

        if (session.IsCompleted)
            throw new InvalidOperationException("Request has already been processed");

        // Envia a decisão para o workflow
        session.SendApprovalDecision(approved, comments);

        // Aguarda o resultado final
        var result = await session.WaitForFinalResultAsync();

        return new DiscountResponse
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


}