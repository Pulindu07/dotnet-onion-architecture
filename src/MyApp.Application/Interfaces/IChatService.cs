namespace MyApp.Application.Interfaces
{
    public interface IChatService
    {
        Task<string> GetChatbotResponseAsync(string userMessage, string conversationId);
    }
}
