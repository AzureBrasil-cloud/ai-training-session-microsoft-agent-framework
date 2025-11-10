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
    public async Task<Result<AudioPreOrderResult>> CreateAsync(CreateAudioPreOrdersRequest request)
    {
        var validationResult = await new CreateAudioPreOrdersRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var error = ErrorHandler.CreateErrorFromValidationResult(validationResult);
            logger.LogInformation("{Method} - {Message}", nameof(CreateAsync), error.Message);
            
            return Result<AudioPreOrderResult>.Failure(error);
        }
        
        var content = await speechService.TranscribeAsync(
            new ApiKeyCredentials(
                configuration["AI:Speech:Endpoint"]!,
                configuration["AI:Speech:Key"]!),
            request.Content,
            request.AudioName);

        var audioPreOder = new AudioPreOrder(
            request.UserEmail,
            request.AudioName,
            request.AudioExtension,
            content.CombinedPhrases.FirstOrDefault()?.Text!);
        
        await context.AudioPreOrders.AddAsync(audioPreOder);
        await context.SaveChangesAsync();
        
        return Result<AudioPreOrderResult>.Success((AudioPreOrderResult)audioPreOder);
    }
}