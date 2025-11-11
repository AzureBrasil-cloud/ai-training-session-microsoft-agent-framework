using ContosoAutoTech.Data.Entities;

namespace ContosoAutoTech.Application.Agents.Models.Requests;

public record AgentRequest(
    string AgentName, 
    string AgentInstructions,
    Feature Feature);