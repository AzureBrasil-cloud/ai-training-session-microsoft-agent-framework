namespace ContosoAutoTech.Application.Agents.Models.Requests;

public record CreateRunRequest(
    string AgentId,
    string ThreadId,
    string Message);