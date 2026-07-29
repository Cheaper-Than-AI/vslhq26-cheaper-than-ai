using Microsoft.Extensions.AI;

namespace CheaperThanAi.Server.Services
{
    public interface IAiService
    {
        public Task<ChatResponse<T>> GetResponse<T>(List<ChatMessage> history);
    }
}
