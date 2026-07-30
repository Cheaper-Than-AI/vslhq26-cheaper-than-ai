using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using OllamaSharp;

namespace CheaperThanAi.Server.Services
{
    public class OllamaAiService : IAiService
    {
        private readonly IChatClient _chatClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public OllamaAiService(IConfiguration config, IServiceScopeFactory serviceScopeFactory)
        {
            string endpoint = config.GetConnectionString("Ollama:Endpoint")
                ?? throw new InvalidOperationException("Ollama endpoint not configured");
            string chatModel = config.GetConnectionString("Ollama:Model")
                ?? throw new InvalidOperationException("Ollama model not configured");

            _chatClient = new ChatClientBuilder(new OllamaApiClient(new Uri(endpoint), chatModel))
                .UseFunctionInvocation()
                .Build();

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
