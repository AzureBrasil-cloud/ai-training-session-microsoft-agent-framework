namespace ContosoAutoTech.Application.AiInference.Models.Requests;

public record ChatCompletionRequest(
    string Instructions,
    string Message);

