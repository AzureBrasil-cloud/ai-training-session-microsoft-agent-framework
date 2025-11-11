namespace CarStockMcp;

public class StockService
{
    private readonly List<CarPart> _parts;
    private readonly List<StockItem> _stock;

    public StockService()
    {
        _parts = new List<CarPart>
        {
            new CarPart { Id = 1, Name = "Filtro de Óleo", Brand = "Honda", Model = "Civic", Price = 45.90m, Category = "Filtros" },
            new CarPart { Id = 2, Name = "Pastilha de Freio Dianteira", Brand = "Honda", Model = "Civic", Price = 189.90m, Category = "Freios" },
            new CarPart { Id = 3, Name = "Vela de Ignição", Brand = "Honda", Model = "Civic", Price = 32.50m, Category = "Motor" },
            new CarPart { Id = 4, Name = "Correia Dentada", Brand = "Honda", Model = "Civic", Price = 156.00m, Category = "Motor" },
            new CarPart { Id = 5, Name = "Filtro de Ar", Brand = "Toyota", Model = "Corolla", Price = 67.80m, Category = "Filtros" },
            new CarPart { Id = 6, Name = "Disco de Freio", Brand = "Toyota", Model = "Corolla", Price = 245.00m, Category = "Freios" },
            new CarPart { Id = 7, Name = "Amortecedor Dianteiro", Brand = "Toyota", Model = "Corolla", Price = 380.00m, Category = "Suspensão" },
            new CarPart { Id = 8, Name = "Bateria 60Ah", Brand = "Toyota", Model = "Corolla", Price = 420.00m, Category = "Elétrica" },
            new CarPart { Id = 9, Name = "Filtro de Combustível", Brand = "Chevrolet", Model = "Onix", Price = 54.90m, Category = "Filtros" },
            new CarPart { Id = 10, Name = "Pastilha de Freio Traseira", Brand = "Chevrolet", Model = "Onix", Price = 125.00m, Category = "Freios" },
            new CarPart { Id = 11, Name = "Radiador", Brand = "Chevrolet", Model = "Onix", Price = 565.00m, Category = "Arrefecimento" },
            new CarPart { Id = 12, Name = "Alternador", Brand = "Chevrolet", Model = "Onix", Price = 680.00m, Category = "Elétrica" },
            new CarPart { Id = 13, Name = "Filtro de Cabine", Brand = "Volkswagen", Model = "Gol", Price = 38.90m, Category = "Filtros" },
            new CarPart { Id = 14, Name = "Bomba de Combustível", Brand = "Volkswagen", Model = "Gol", Price = 310.00m, Category = "Combustível" },
            new CarPart { Id = 15, Name = "Kit Embreagem", Brand = "Volkswagen", Model = "Gol", Price = 890.00m, Category = "Transmissão" },
            new CarPart { Id = 16, Name = "Bobina de Ignição", Brand = "Volkswagen", Model = "Gol", Price = 178.00m, Category = "Motor" },
            new CarPart { Id = 17, Name = "Filtro de Óleo", Brand = "Hyundai", Model = "HB20", Price = 42.00m, Category = "Filtros" },
            new CarPart { Id = 18, Name = "Tensor da Correia", Brand = "Hyundai", Model = "HB20", Price = 145.00m, Category = "Motor" },
            new CarPart { Id = 19, Name = "Sensor de Oxigênio", Brand = "Hyundai", Model = "HB20", Price = 285.00m, Category = "Elétrica" },
            new CarPart { Id = 20, Name = "Mangueira do Radiador", Brand = "Hyundai", Model = "HB20", Price = 89.90m, Category = "Arrefecimento" },
        };

        _stock = new List<StockItem>
        {
            new StockItem { PartId = 1, Quantity = 45, MinimumStock = 10, Location = "Prateleira A1", LastUpdated = DateTime.Now.AddDays(-2) },
            new StockItem { PartId = 2, Quantity = 8, MinimumStock = 15, Location = "Prateleira A2", LastUpdated = DateTime.Now.AddDays(-1) },
            new StockItem { PartId = 3, Quantity = 120, MinimumStock = 30, Location = "Prateleira B1", LastUpdated = DateTime.Now.AddDays(-3) },
            new StockItem { PartId = 4, Quantity = 25, MinimumStock = 10, Location = "Prateleira B2", LastUpdated = DateTime.Now.AddDays(-5) },
            new StockItem { PartId = 5, Quantity = 0, MinimumStock = 20, Location = "Prateleira C1", LastUpdated = DateTime.Now.AddDays(-10) },
            new StockItem { PartId = 6, Quantity = 18, MinimumStock = 12, Location = "Prateleira C2", LastUpdated = DateTime.Now.AddDays(-1) },
            new StockItem { PartId = 7, Quantity = 5, MinimumStock = 8, Location = "Prateleira D1", LastUpdated = DateTime.Now.AddDays(-7) },
            new StockItem { PartId = 8, Quantity = 22, MinimumStock = 10, Location = "Prateleira D2", LastUpdated = DateTime.Now },
            new StockItem { PartId = 9, Quantity = 65, MinimumStock = 25, Location = "Prateleira E1", LastUpdated = DateTime.Now.AddDays(-2) },
            new StockItem { PartId = 10, Quantity = 3, MinimumStock = 15, Location = "Prateleira E2", LastUpdated = DateTime.Now.AddDays(-4) },
            new StockItem { PartId = 11, Quantity = 12, MinimumStock = 5, Location = "Prateleira F1", LastUpdated = DateTime.Now.AddDays(-1) },
            new StockItem { PartId = 12, Quantity = 7, MinimumStock = 6, Location = "Prateleira F2", LastUpdated = DateTime.Now.AddDays(-3) },
            new StockItem { PartId = 13, Quantity = 88, MinimumStock = 30, Location = "Prateleira G1", LastUpdated = DateTime.Now.AddDays(-2) },
            new StockItem { PartId = 14, Quantity = 0, MinimumStock = 8, Location = "Prateleira G2", LastUpdated = DateTime.Now.AddDays(-15) },
            new StockItem { PartId = 15, Quantity = 14, MinimumStock = 5, Location = "Prateleira H1", LastUpdated = DateTime.Now.AddDays(-1) },
            new StockItem { PartId = 16, Quantity = 42, MinimumStock = 20, Location = "Prateleira H2", LastUpdated = DateTime.Now },
            new StockItem { PartId = 17, Quantity = 2, MinimumStock = 15, Location = "Prateleira I1", LastUpdated = DateTime.Now.AddDays(-6) },
            new StockItem { PartId = 18, Quantity = 31, MinimumStock = 12, Location = "Prateleira I2", LastUpdated = DateTime.Now.AddDays(-2) },
            new StockItem { PartId = 19, Quantity = 9, MinimumStock = 8, Location = "Prateleira J1", LastUpdated = DateTime.Now.AddDays(-4) },
            new StockItem { PartId = 20, Quantity = 54, MinimumStock = 20, Location = "Prateleira J2", LastUpdated = DateTime.Now.AddDays(-1) },
        };

        // Associar peças ao estoque
        foreach (var stock in _stock)
        {
            stock.Part = _parts.First(p => p.Id == stock.PartId);
        }
    }

    public IEnumerable<StockItem> GetAllStock() => _stock;

    public IEnumerable<StockItem> GetStockByBrand(string brand)
    {
        return _stock.Where(s => s.Part.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<StockItem> GetStockByModel(string model)
    {
        return _stock.Where(s => s.Part.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<StockItem> GetLowStock()
    {
        return _stock.Where(s => s.IsLowStock);
    }

    public IEnumerable<StockItem> GetOutOfStock()
    {
        return _stock.Where(s => s.Quantity == 0);
    }

    public StockItem? GetStockByPartId(int partId)
    {
        return _stock.FirstOrDefault(s => s.PartId == partId);
    }

    public bool UpdateStock(int partId, int quantity)
    {
        var stock = GetStockByPartId(partId);
        if (stock != null)
        {
            stock.Quantity = quantity;
            stock.LastUpdated = DateTime.Now;
            return true;
        }
        return false;
    }

    public IEnumerable<string> GetAvailableBrands()
    {
        return _parts.Select(p => p.Brand).Distinct().OrderBy(b => b);
    }

    public IEnumerable<string> GetAvailableModels()
    {
        return _parts.Select(p => p.Model).Distinct().OrderBy(m => m);
    }
}