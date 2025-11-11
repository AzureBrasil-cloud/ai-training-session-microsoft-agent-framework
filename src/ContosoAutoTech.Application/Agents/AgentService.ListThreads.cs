using System.Diagnostics;
using ContosoAutoTech.Application.Agents.Models.Requests;
using ContosoAutoTech.Application.Agents.Models.Results;
using Microsoft.EntityFrameworkCore;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    public async Task<Result<IList<ThreadResult>>> ListThreadsByFeatureAsync(ListThreadsByFeatureRequest request)
    {
        using Activity? activity = ActivitySource.StartActivity();
        
        var threads = await context.Threads
            .AsNoTracking()
            .Where(t=> t.Feature == request.Feature)
            .Where(t => !string.IsNullOrWhiteSpace(t.FirstTruncatedMessage))
            .ToListAsync();
        
        var result = threads.Select(x => new ThreadResult(
            x.Id, 
            x.FirstTruncatedMessage)).ToList();

        return result;
    }
}

