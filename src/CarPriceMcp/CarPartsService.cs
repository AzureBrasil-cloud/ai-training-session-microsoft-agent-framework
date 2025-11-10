using CarPriceMcp;

public class CarPartsService
{
    private readonly List<CarPart> _partsCatalog = new()
    {
        // Honda Civic
        new CarPart { Name = "Filtro de Óleo", Brand = "Honda", Model = "Civic", Price = 45.90m, Category = "Filtros" },
        new CarPart { Name = "Pastilha de Freio Dianteira", Brand = "Honda", Model = "Civic", Price = 189.90m, Category = "Freios" },
        new CarPart { Name = "Vela de Ignição", Brand = "Honda", Model = "Civic", Price = 32.50m, Category = "Motor" },
        new CarPart { Name = "Correia Dentada", Brand = "Honda", Model = "Civic", Price = 156.00m, Category = "Motor" },
        
        // Toyota Corolla
        new CarPart { Name = "Filtro de Ar", Brand = "Toyota", Model = "Corolla", Price = 67.80m, Category = "Filtros" },
        new CarPart { Name = "Disco de Freio", Brand = "Toyota", Model = "Corolla", Price = 245.00m, Category = "Freios" },
        new CarPart { Name = "Amortecedor Dianteiro", Brand = "Toyota", Model = "Corolla", Price = 380.00m, Category = "Suspensão" },
        new CarPart { Name = "Bateria 60Ah", Brand = "Toyota", Model = "Corolla", Price = 420.00m, Category = "Elétrica" },
        
        // Chevrolet Onix
        new CarPart { Name = "Filtro de Combustível", Brand = "Chevrolet", Model = "Onix", Price = 54.90m, Category = "Filtros" },
        new CarPart { Name = "Pastilha de Freio Traseira", Brand = "Chevrolet", Model = "Onix", Price = 125.00m, Category = "Freios" },
        new CarPart { Name = "Radiador", Brand = "Chevrolet", Model = "Onix", Price = 565.00m, Category = "Arrefecimento" },
        new CarPart { Name = "Alternador", Brand = "Chevrolet", Model = "Onix", Price = 680.00m, Category = "Elétrica" },
        
        // Volkswagen Gol
        new CarPart { Name = "Filtro de Cabine", Brand = "Volkswagen", Model = "Gol", Price = 38.90m, Category = "Filtros" },
        new CarPart { Name = "Bomba de Combustível", Brand = "Volkswagen", Model = "Gol", Price = 310.00m, Category = "Combustível" },
        new CarPart { Name = "Kit Embreagem", Brand = "Volkswagen", Model = "Gol", Price = 890.00m, Category = "Transmissão" },
        new CarPart { Name = "Bobina de Ignição", Brand = "Volkswagen", Model = "Gol", Price = 178.00m, Category = "Motor" },
        
        // Hyundai HB20
        new CarPart { Name = "Filtro de Óleo", Brand = "Hyundai", Model = "HB20", Price = 42.00m, Category = "Filtros" },
        new CarPart { Name = "Tensor da Correia", Brand = "Hyundai", Model = "HB20", Price = 145.00m, Category = "Motor" },
        new CarPart { Name = "Sensor de Oxigênio", Brand = "Hyundai", Model = "HB20", Price = 285.00m, Category = "Elétrica" },
        new CarPart { Name = "Mangueira do Radiador", Brand = "Hyundai", Model = "HB20", Price = 89.90m, Category = "Arrefecimento" },
        
        // Peças Universais
        new CarPart { Name = "Óleo de Motor 5W30", Brand = "Universal", Model = "Todos", Price = 45.90m, Category = "Lubrificantes" },
        new CarPart { Name = "Aditivo de Radiador", Brand = "Universal", Model = "Todos", Price = 28.00m, Category = "Arrefecimento" },
        new CarPart { Name = "Lâmpada H7", Brand = "Universal", Model = "Todos", Price = 25.90m, Category = "Elétrica" },
    };

    public IReadOnlyList<CarPart> GetAllParts() => _partsCatalog.AsReadOnly();

    public IEnumerable<CarPart> GetPartsByBrand(string brand)
    {
        return _partsCatalog.Where(p =>
            p.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<CarPart> GetPartsByModel(string model)
    {
        return _partsCatalog.Where(p =>
            p.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<string> GetAvailableBrands()
    {
        return _partsCatalog.Select(p => p.Brand).Distinct().OrderBy(b => b);
    }

    public IEnumerable<string> GetAvailableModels()
    {
        return _partsCatalog.Select(p => p.Model).Distinct().OrderBy(m => m);
    }
}
