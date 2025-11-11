using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Extensions.AI;

namespace ContosoAutoTech.Infrastructure.AiInference;

public sealed partial class AiInferenceService
{
    public async Task<ChatResponse> CompleteAsync(Credentials credentials,
        string instructions,
        string content)
    {
        var openAiClient = CreateChatClient(credentials);

        IEnumerable<ChatMessage> messages =
        [
            new(ChatRole.System, instructions),
            new(ChatRole.User, content)
        ];
        
        var result = await openAiClient.GetResponseAsync(messages);

        return result;
    }
}