using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace EvolvingFunctionApp
{
    public class EvolvingFa
    {
        private readonly ILogger<EvolvingFa> _logger;
        private const string ConfigFile = "efa_config.txt";

        public EvolvingFa(ILogger<EvolvingFa> logger)
        {
            _logger = logger;

            // Initialize config file if it doesn't exist
            if (!File.Exists(ConfigFile))
            {
                File.WriteAllText(ConfigFile, "default_value");
            }
        }

        [Function("EvolvingFa")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Read current "environment variable" from file
            var currentValue = await File.ReadAllTextAsync(ConfigFile);

            return new OkObjectResult($"Current config value: {currentValue}");
        }
    }
}