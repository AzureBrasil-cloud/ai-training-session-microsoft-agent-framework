namespace CarStockMcp;

public class StockItem
{
    public int PartId { get; set; }
    public CarPart Part { get; set; }
    public int Quantity { get; set; }
    public int MinimumStock { get; set; }
    public string Location { get; set; }
    public DateTime LastUpdated { get; set; }

    public bool IsLowStock => Quantity <= MinimumStock;
    public string StockStatus => Quantity == 0 ? "Esgotado" : 
        IsLowStock ? "Estoque Baixo" : 
        "Disponível";
}