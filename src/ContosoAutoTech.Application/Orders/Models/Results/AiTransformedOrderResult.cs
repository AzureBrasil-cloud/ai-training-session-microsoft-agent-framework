using ContosoAutoTech.Data.Entities;

namespace ContosoAutoTech.Application.Orders.Models.Results;

public class AiTransformedOrderResult
{
    public Size Size { get; set; }
    public string[] Extras { get; set; } = null!; 
    
    public static implicit operator AiTransformedOrderResult(AiTransformedOrder order)
    {
        return new AiTransformedOrderResult
        {
            Size = order.Size,
            Extras = order.Extras
        };
    }
}