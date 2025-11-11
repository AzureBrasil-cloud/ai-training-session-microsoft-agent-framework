using ContosoAutoTech.Application.Agents.Models.Results;

namespace ContosoAutoTech.Application.AiInference.Models.Results;

public record ChatCompletionResult(
    string Content,
    TokenUsage? Usage);

