using System.Text.Json;
using ContosoAutoTech.Application.Agents.Models.Results;
using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    public async Task<Result<ThreadResult>> CreateThreadAsync()
    {
        var foundryEndpoint = configuration["AiFoundry:Endpoint"]!;
        var foudryApiKey = configuration["AiFoundry:ApiKey"]!;
        var model = configuration["AiFoundry:ChatModel"]!;
        
        var credentials = new Credentials(foundryEndpoint, foudryApiKey, model);
       
        var aiThread = aiAgentService.CreateThread(
            credentials,
            string.Empty,
            string.Empty);
        
        // State to string
        // Serialize the thread state
        string serializedJson = aiThread.Serialize(JsonSerializerOptions.Web).GetRawText();

        var thread = new ContosoAutoTech.Data.Entities.Thread(serializedJson);
        
        await context.Threads.AddAsync(thread);
        await context.SaveChangesAsync();
        
        return new ThreadResult(thread.Id);
    }
}