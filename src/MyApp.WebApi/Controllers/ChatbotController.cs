using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyApp.Application.Models.Requests;
using MyApp.Application.Models.Responses;

namespace MyApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ChatbotController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ChatbotController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            var azureConfig = _configuration.GetSection("AzureChatbot");
            
            var directLineSecret = Environment.GetEnvironmentVariable("ChatBotDirectLineSecret") ?? azureConfig["DirectLineSecret"];
            var botEndpoint = azureConfig["BotEndpoint"];

            // Reset headers before each request
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", directLineSecret);

            // Step 1: Get conversation ID
            var conversationResponse = await StartConversation(botEndpoint);
            
            // Step 2: Send message to bot
            var replyResponse = await SendMessageToBot(
                conversationResponse.ConversationId, 
                conversationResponse.StreamUrl, 
                request.Message, 
                directLineSecret
            );

            return Ok(new { reply = replyResponse });
        }

        private async Task<ConversationResponse> StartConversation(string botEndpoint)
        {
            var response = await _httpClient.PostAsync(
                $"{botEndpoint}/conversations", 
                new StringContent("")
            );

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ConversationResponse>(content);
        }

        private async Task<string> SendMessageToBot(
            string conversationId, 
            string streamUrl, 
            string message, 
            string directLineSecret)
        {
            // Reset headers and set Authorization
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", directLineSecret);

            var messagePayload = new 
            {
                type = "message",
                from = new { id = "user1" },
                text = message
            };

            var response = await _httpClient.PostAsync(
                $"{streamUrl}/conversations/{conversationId}/activities", 
                new StringContent(
                    JsonConvert.SerializeObject(messagePayload), 
                    Encoding.UTF8, 
                    "application/json"
                )
            );

            // Process and return bot's reply
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode){
                throw new Exception($"Failed to send message: {response.StatusCode} - {content}");
            }

            // Extract bot response from the response payload
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(content);
            var activities = jsonResponse?.activities;

            if (activities != null && activities.Count > 0)
            {
                foreach (var activity in activities)
                {
                    if ((string)activity.type == "message" && !string.IsNullOrWhiteSpace((string)activity.text))
                    {
                        return (string)activity.text;
                    }
                }
            }

            return "No response received from the bot.";
        }

        public class ChatRequest 
        {
            public string Message { get; set; }
        }

        public class ConversationResponse 
        {
            public string ConversationId { get; set; }
            public string StreamUrl { get; set; }
        }
    }
}