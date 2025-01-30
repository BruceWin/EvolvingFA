using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EvolvingFunctionApp
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(
            HttpClient httpClient,
            IConfiguration config,
            ILogger<OpenAIService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;

            // Configure OpenAI API client
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _config["OPENAI_API_KEY"]);
        }

        public async Task<string> GetDapiResponseAsync()
        {
            try
            {
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = "Generate a unique philosophical quote about technology. Keep it under 15 words."
                        }
                    }
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "chat/completions",
                    requestBody
                );

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"OpenAI API error: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadFromJsonAsync<JsonDocument>();
                return content.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get DAPI response");
                return null;
            }
        }
    }
}