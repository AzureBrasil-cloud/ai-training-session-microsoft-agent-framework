using ContosoAutoTech.Data.Entities;
using ContosoAutoTech.Application.Orders.Models.Requests;
using ContosoAutoTech.Application.Orders.Models.Requests.Validators;
using ContosoAutoTech.Application.Orders.Models.Results;
using ContosoAutoTech.Infrastructure.Azure.Shared;
using Microsoft.Extensions.Logging;
using PowerPilotChat.Application;

namespace ContosoAutoTech.Application.Orders;

public partial class OrderService
{
    public async Task<Result<ImagePreOrderResult>> CreateAsync(CreateImagePreOrdersRequest request)
    {
        var validationResult = await new CreateImagePreOrdersRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var error = ErrorHandler.CreateErrorFromValidationResult(validationResult);
            logger.LogInformation("{Method} - {Message}", nameof(CreateAsync), error.Message);
            
            return Result<ImagePreOrderResult>.Failure(error);
        }
        
        var keyValuePairs = await documentIntelligenceService.AnalyseAsync(
            new ApiKeyCredentials(
                configuration["AI:DocumentIntelligence:Endpoint"]!,
                configuration["AI:DocumentIntelligence:Key"]!),
            request.Content);

        var imagePreOder = new ImagePreOrder(
            request.UserEmail, 
            keyValuePairs,
            request.Name,
            request.Extension);
        
        await context.ImagePreOrders.AddAsync(imagePreOder);
        await context.SaveChangesAsync();
        
        return Result<ImagePreOrderResult>.Success((ImagePreOrderResult)imagePreOder);
    }
}