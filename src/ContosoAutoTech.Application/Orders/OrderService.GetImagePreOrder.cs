using ContosoAutoTech.Data.Entities;
using ContosoAutoTech.Application.Orders.Models.Query;
using ContosoAutoTech.Application.Orders.Models.Results;
using ContosoAutoTech.Infrastructure.Azure.Shared;
using Microsoft.EntityFrameworkCore;
using PowerPilotChat.Application;

namespace ContosoAutoTech.Application.Orders;

public partial class OrderService
{
    private string InstructionsImage => """
                                   Extract the order information;
                                   """;
    
    public async Task<ImagePreOrderResult?> GetByIdAsync(GetImagePreOrderQuery query)
    {
        var imagePreOrder = await context.ImagePreOrders
            .FirstOrDefaultAsync(x => x.Id == query.Id);

        if (imagePreOrder is null)
        {
            return null;
        }
        
        var result = (ImagePreOrderResult)imagePreOrder;

        if (!query.ApplyAiTransformation) return result;
        
        var inferenceResult = await aiInferenceService.CompleteAsync<AiTransformedOrder>(
            new ApiKeyCredentials(
                configuration["AI:Inference:Endpoint"]!,
                configuration["AI:Inference:Key"]!),
            configuration["AI:Inference:Model"]!,
            InstructionsImage,
            string.Join("\n", imagePreOrder.KeyValuePairs));
            
        result.AiTransformedOrder = inferenceResult;

        return result;
    }
}