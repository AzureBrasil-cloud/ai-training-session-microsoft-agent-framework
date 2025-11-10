using ContosoAutoTech.Data.Entities;

namespace ContosoAutoTech.Application.Orders.Models.Requests;

public record CreateOrdersRequest(
    string UserEmail,
    Size Size,
    string[] Extras);