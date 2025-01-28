// MyApp.Infrastructure/Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using MyApp.Application.Interfaces;

namespace MyApp.Infrastructure.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string userMessage, string conversationId)
        {
            var botReply = await _chatService.GetChatbotResponseAsync(userMessage, conversationId);
            await Clients.Caller.SendAsync("ReceiveMessage", botReply);
        }
    }
}