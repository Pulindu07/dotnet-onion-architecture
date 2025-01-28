using MyApp.Application.Interfaces;
using MyApp.Application.Core.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ChatService(IHttpClientFactory httpClientFactory, IConfiguration configuration )
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string?> GetChatbotResponseAsync(string userMessage, string conversationId)
        {
            var systemMessage= _configuration["ChatbotSettings:SystemMessage"];

            try 
            {
                var client = _httpClientFactory.CreateClient("Open_AI");

                var requestContent = new
                {
                    model = "gpt-4",
                    messages = new[]
                    {
                        new { role = "system", content = systemMessage },
                        new { role = "user", content = userMessage }
                    }
                };

                var response = await client.PostAsJsonAsync("chat/completions", requestContent);
                
                response.EnsureSuccessStatusCode(); // Throws exception for non-success status codes

                var responseData = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                return responseData?.Choices?[0]?.Message?.Content;
            }
            catch (HttpRequestException ex)
            {
                // Log the detailed error
                Console.WriteLine($"OpenAI API Request Error: {ex.Message}");
                throw new Exception("Failed to connect to OpenAI API", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public class OpenAIResponse
        {
            public Choice[]? Choices { get; set; }
        }

        public class Choice
        {
            public Message? Message { get; set; }
        }

        public class Message
        {
            public string? Content { get; set; }
        }
            }
}
