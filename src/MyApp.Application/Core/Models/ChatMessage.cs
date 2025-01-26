namespace MyApp.Application.Core.Models
{
    public class ChatMessage
    {
        public string Role { get; set; } // "user" or "bot"
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}