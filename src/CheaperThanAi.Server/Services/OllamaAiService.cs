using Microsoft.Extensions.AI;
using OllamaSharp;

namespace CheaperThanAi.Server.Services
{
    public class OllamaAiService : IAiService
    {
        private IChatClient _chatClient;

        public OllamaAiService()
        {
            string endpoint = String.Empty;
            string chatModel = String.Empty;

            _chatClient = new ChatClientBuilder(new OllamaApiClient(new Uri(endpoint), chatModel))
                .UseFunctionInvocation()
                .Build();
        }
        public async Task<ChatResponse<T>> GetResponse<T>(List<ChatMessage> history)
        {
            return await _chatClient.GetResponseAsync<T>(history);
        }
    }
}
