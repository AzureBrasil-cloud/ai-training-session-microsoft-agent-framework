using System.Text.Json;
using ContosoAutoTech.Application.Agents.Models.Results;
using Microsoft.Extensions.Logging;

namespace ContosoAutoTech.Application.Agents;

public partial class AgentService
{
    public async Task<Result<ThreadMessagesResult>> GetThreadMessagesAsync(Guid threadId)
    {
        // Get the thread from database
        var thread = await context.Threads.FindAsync(threadId);

        if (thread is null)
        {
            var error = Errors.EntityNotFound(nameof(Thread), threadId.ToString());
            logger.LogInformation("{Method} - {Message}", nameof(GetThreadMessagesAsync), error.Message);
            return Result<ThreadMessagesResult>.Failure(error);
        }

        var state = thread.State;

        try
        {
            // Parse the JSON state
            using var document = JsonDocument.Parse(state);
            var root = document.RootElement;

            var messageResults = new List<MessageResult>();

            // Navigate to storeState.messages array
            if (root.TryGetProperty("storeState", out var storeState) &&
                storeState.TryGetProperty("messages", out var messages) &&
                messages.ValueKind == JsonValueKind.Array)
            {
                foreach (var message in messages.EnumerateArray())
                {
                    // Get role
                    var roleStr = message.GetProperty("role").GetString();
                    var role = roleStr?.Equals("user", StringComparison.OrdinalIgnoreCase) == true 
                        ? Role.User 
                        : Role.Agent;

                    // Get content from contents array
                    var content = string.Empty;
                    if (message.TryGetProperty("contents", out var contents) &&
                        contents.ValueKind == JsonValueKind.Array)
                    {
                        var contentParts = new List<string>();
                        foreach (var contentItem in contents.EnumerateArray())
                        {
                            if (contentItem.TryGetProperty("text", out var textProp))
                            {
                                contentParts.Add(textProp.GetString() ?? string.Empty);
                            }
                        }
                        content = string.Join("\n", contentParts);
                    }

                    messageResults.Add(new MessageResult(role, content, null));
                }
            }

            return Result<ThreadMessagesResult>.Success(new ThreadMessagesResult(messageResults
                .Where(x => !string.IsNullOrWhiteSpace(x.Content))));
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Error parsing thread state JSON for thread {ThreadId}", threadId);
            var error = Errors.Unexpected();
            return Result<ThreadMessagesResult>.Failure(error);
        }
    }
}

