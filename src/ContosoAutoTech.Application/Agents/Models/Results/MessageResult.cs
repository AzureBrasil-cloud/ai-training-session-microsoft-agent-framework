namespace ContosoAutoTech.Application.Agents.Models.Results;

public record MessageResult(Role Role, string Content, TokenUsage? Usage = null);

public record TokenUsage(long? Input, long? Output, long? Total);

public enum Role
{
    User = 1,
    Agent
}

