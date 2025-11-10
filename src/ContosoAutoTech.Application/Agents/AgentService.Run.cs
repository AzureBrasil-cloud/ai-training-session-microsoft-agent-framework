// using System.Diagnostics;
// using ContosoAutoTech.Application.Agents.Models.Requests;
// using ContosoAutoTech.Application.Agents.Models.Requests.Validators;
// using ContosoAutoTech.Application.Agents.Models.Results;
// using ContosoAutoTech.Infrastructure.AIAgent.Models;
// using Microsoft.Extensions.Logging;
// using PowerPilotChat.Application;
//
// namespace ContosoAutoTech.Application.Agents;
//
// public partial class AgentService
// {
//     private static readonly ActivitySource ActivitySource =  InstrumentationConfig.ActivitySource;
//     
//     public async Task<Result<MessageResult>> RunAsync(CreateRunRequest request)
//     {
//         var validationResult = await new CreateRunRequestValidator().ValidateAsync(request);
//
//         if (!validationResult.IsValid)
//         {
//             var error = ErrorHandler.CreateErrorFromValidationResult(validationResult);
//             logger.LogInformation("{Method} - {Message}", nameof(CreateAsync), error.Message);
//             
//             return Result<MessageResult>.Failure(error);
//         }
//
//         logger.LogInformation("Starting run ...");
//         
//         using Activity activity = ActivitySource.StartActivity("Run");
//         
//         var result = await aiAgentService.CreateRunAsync(
//             CreateCredentials(),
//             request.ThreadId,
//             request.Message,
//             request.AgentId);
//
//         activity!.SetTag("InputTokenCount", result.InputTokenCount.ToString());
//         activity!.SetTag("OutputTokenCount", result.OutputTokenCount.ToString());
//         
//         logger.LogInformation("Run completed ...");
//         
//         return Result<MessageResult>.Success(new MessageResult(result.Message.Role, result.Message.Content));
//     }
// }