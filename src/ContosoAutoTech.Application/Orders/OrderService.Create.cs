using ContosoAutoTech.Data.Entities;
using ContosoAutoTech.Application.Orders.Models.Requests;
using ContosoAutoTech.Application.Orders.Models.Requests.Validators;
using ContosoAutoTech.Application.Orders.Models.Results;
using Microsoft.Extensions.Logging;
using PowerPilotChat.Application;

namespace ContosoAutoTech.Application.Orders;

public partial class OrderService
{
    public async Task<Result<OrderResult>> CreateAsync(CreateOrdersRequest request)
    {
        var validationResult = await new CreateOrdersRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var error = ErrorHandler.CreateErrorFromValidationResult(validationResult);
            logger.LogInformation("{Method} - {Message}", nameof(CreateAsync), error.Message);
            
            return Result<OrderResult>.Failure(error);
        }

        var order = new Order(
            request.UserEmail,
            request.Size,
            request.Extras);
        
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Result<OrderResult>.Success((OrderResult)order);
    }
}