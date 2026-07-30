using CheaperThanAi.Server.Services;

namespace CheaperThanAi.Server.Extensions
{
    public static class ChatClientExtensions
    {
        public static IServiceCollection UseOllamaClient(this IServiceCollection app)
        {
            app.AddSingleton<IAiService, OllamaAiService>();
            return app;
        }

        public static IServiceCollection UseAzureClient(this IServiceCollection app)
        {
            app.AddSingleton<IAiService, AzureAiService>();
            return app;
        }
    }
}
