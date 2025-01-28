namespace MyApp.Application.Models.Requests
{
    public class ChatRequest
    {
        public required string Content { get; set; }
        public required string ConversationId { get; set; }
    }
}
