namespace ContosoAutoTech.Application.Agents.Models.Requests;

public record CreateRunRequest(
    string AgentName,
    string AgentInstructions,
    string ThreadId,
    string Message);