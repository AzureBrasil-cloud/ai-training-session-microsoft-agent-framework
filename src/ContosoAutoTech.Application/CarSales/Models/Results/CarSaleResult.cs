namespace ContosoAutoTech.Application.CarSales.Models.Results;

public record CarSaleResult(
    Guid Id,
    string ImageUrl,
    string Model,
    string LicensePlate,
    string Color,
    decimal Price,
    decimal? ReferencePrice,
    string Description,
    string AgentConsideration,
    List<string> Strengths,
    List<string> Weaknesses,
    DateTime CreatedAt
);

