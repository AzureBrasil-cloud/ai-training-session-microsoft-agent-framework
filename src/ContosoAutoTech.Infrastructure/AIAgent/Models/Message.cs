namespace ContosoAutoTech.Infrastructure.AIAgent.Models;

public record Message(
    string Id, 
    string Role, 
    string Content);