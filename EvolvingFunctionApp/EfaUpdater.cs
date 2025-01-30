using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EvolvingFunctionApp
{
    public class EfaUpdater
    {
        private readonly ILogger<EfaUpdater> _logger;
        private readonly OpenAIService _openAIService;
        public static string LatestResponse { get; private set; } = "INITIAL_VALUE";

        public EfaUpdater(
            ILogger<EfaUpdater> logger,
            OpenAIService openAIService)
        {
            _logger = logger;
            _openAIService = openAIService;
        }

        [Function("EfaUpdater")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo)
        {
            var response = await _openAIService.GetDapiResponseAsync();

            if (!string.IsNullOrEmpty(response))
            {
                LatestResponse = response;
                _logger.LogInformation($"Updated DAPI response: {LatestResponse}");
            }
        }
    }
}