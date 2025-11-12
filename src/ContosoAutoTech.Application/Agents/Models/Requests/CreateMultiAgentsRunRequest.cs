using ContosoAutoTech.Data.Entities;

namespace ContosoAutoTech.Application.Agents.Models.Requests;

public record CreateMultiAgentsRunRequest(
    Feature Feature,
    string AgentName,
    string AgentInstructions,
    string ThreadId,
    string Message,
    AgentRequest[] Agents
);