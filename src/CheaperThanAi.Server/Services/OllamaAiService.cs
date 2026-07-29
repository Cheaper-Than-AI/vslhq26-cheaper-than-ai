using Microsoft.Extensions.AI;
using OllamaSharp;

namespace CheaperThanAi.Server.Services
{
    public class OllamaAiService : IAiService
    {
        private IChatClient _chatClient;

        public OllamaAiService(IConfiguration config)
        {
            string endpoint = config.GetConnectionString("Ollama:Endpoint")
                ?? throw new InvalidOperationException("Ollama endpoint not configured");
            string chatModel = config.GetConnectionString("Ollama:Model")
                ?? throw new InvalidOperationException("Ollama model not configured");

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
