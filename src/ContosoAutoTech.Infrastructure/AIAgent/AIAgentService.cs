using Azure;
using Azure.AI.OpenAI;
using ContosoAutoTech.Infrastructure.Email;
using ContosoAutoTech.Infrastructure.Shared;
using OpenAI.Chat;

namespace ContosoAutoTech.Infrastructure.AIAgent;

public partial class AiAgentService(EmailService emailService)
{
    
    private ChatClient CreateAgentsClient(Credentials credentials)
    {
        return new AzureOpenAIClient(  
                new Uri(credentials.FoundryEndpoint),   
                new AzureKeyCredential(credentials.FoundryApiKey))  
            .GetChatClient(credentials.ModelDeploymentName);      
    }
}