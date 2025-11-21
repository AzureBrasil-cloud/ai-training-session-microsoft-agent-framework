public record Product
{
    public Guid Id { get; init; }
    public string ProductCode { get; init; } = "";
    public string Name { get; init; } = "";
    public string Brand { get; init; } = "";
    public string Model { get; init; } = "";
}