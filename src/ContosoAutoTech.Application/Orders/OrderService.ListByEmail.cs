using ContosoAutoTech.Common;
using ContosoAutoTech.Application.Orders.Models.Query;
using ContosoAutoTech.Application.Orders.Models.Results;
using Microsoft.EntityFrameworkCore;

namespace ContosoAutoTech.Application.Orders;

public partial class OrderService
{
    public async Task<IEnumerable<OrderResult>> ListByEmail(ListByEmailQuery query)
    {
        var queryable =  context.Orders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.UserEmail))
        {
            queryable = queryable.Where(order => order.UserEmail == query.UserEmail.NormalizeEmail());
        }

        var entities = await queryable.ToListAsync();
        
        return entities.Select(x => (OrderResult)x);
    }
}