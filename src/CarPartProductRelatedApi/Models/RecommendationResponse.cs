namespace CarPartProductRelated.Api.Models;

public record RecommendationResponse(string ProductCode, string Name, IEnumerable<Product> Related);