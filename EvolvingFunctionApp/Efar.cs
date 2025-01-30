using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace EvolvingFunctionApp
{
    public class Efar
    {
        [Function("Efar")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            return new OkObjectResult($"Latest DAPI value: {EfaUpdater.LatestResponse}");
        }
    }
}