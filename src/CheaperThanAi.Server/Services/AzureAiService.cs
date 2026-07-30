using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;

namespace CheaperThanAi.Server.Services
{
    public class AzureAiService : IAiService
    {
        private readonly IChatClient _chatClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AzureAiService(IConfiguration config, IServiceScopeFactory serviceScopeFactory)
        {
            string endpoint = config["AzureOpenAI:Endpoint"]
                ?? throw new InvalidOperationException(
                    "Missing 'AzureOpenAI:Endpoint'. Run: dotnet user-secrets set \"AzureOpenAI:Endpoint\" \"https://YOUR-RESOURCE.openai.azure.com/\"");
            string key = config["AzureOpenAI:Key"]
                ?? throw new InvalidOperationException(
                    "Missing 'AzureOpenAI:Key'. Run: dotnet user-secrets set \"AzureOpenAI:Key\" \"YOUR-KEY\"");
            string chatModel = "gpt-5.4-mini";

            var options = new AzureOpenAIClientOptions(
    AzureOpenAIClientOptions.ServiceVersion.V2024_10_21);

            _chatClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key), options)
            .GetChatClient(chatModel)
            .AsIChatClient();

            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<ChatResponse<T>> GetResponse<T>(List<ChatMessage> history)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var ticketTools = scope.ServiceProvider.GetRequiredService<ITTicketTools>();

            var options = new ChatOptions
            {
                Tools =
                [
                    AIFunctionFactory.Create(ticketTools.CreateITTicket),
                // one entry per method you want the model to call — swap in your real method names
            ]
            };
            return await _chatClient.GetResponseAsync<T>(history, options);
        }
    }
}
