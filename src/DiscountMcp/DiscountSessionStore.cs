using System.Collections.Concurrent;

namespace DiscountMcp;

public class DiscountSessionStore
{
    private readonly ConcurrentDictionary<string, DiscountWorkflowSession> _sessions = new();

    public void Add(string sessionId, DiscountWorkflowSession session)
        => _sessions[sessionId] = session;

    public bool TryGet(string sessionId, out DiscountWorkflowSession? session)
        => _sessions.TryGetValue(sessionId, out session);

    public IEnumerable<DiscountWorkflowSession> GetPendingApprovals()
        => _sessions.Values.Where(s => s.RequiresApproval && !s.IsCompleted);

    public IReadOnlyDictionary<string, DiscountWorkflowSession> Sessions => _sessions;
}