namespace CarPartProduct.Models;

public class Products
{
    public static IEnumerable<Product> Items => new List<Product>
    {
        new() { Id = Guid.NewGuid(), ProductCode = "WH001", Name = "Filtro de óleo", Brand = "Honda", Model = "Civic" },
        new() { Id = Guid.NewGuid(), ProductCode = "CM002", Name = "Pastilha de freio", Brand = "Chevrolet", Model = "Onix" },
        new() { Id = Guid.NewGuid(), ProductCode = "BS003", Name = "Amortecedor traseiro", Brand = "Bosch", Model = "Gol" },
        new() { Id = Guid.NewGuid(), ProductCode = "NS004", Name = "Bateria 60Ah", Brand = "Nissan", Model = "Versa" },
        new() { Id = Guid.NewGuid(), ProductCode = "WB005", Name = "Palheta do limpador", Brand = "Volkswagen", Model = "Polo" },
        new() { Id = Guid.NewGuid(), ProductCode = "DL006", Name = "Filtro de ar", Brand = "Fiat", Model = "Argo" },
        new() { Id = Guid.NewGuid(), ProductCode = "PC007", Name = "Correia dentada", Brand = "Peugeot", Model = "208" }
    };

}