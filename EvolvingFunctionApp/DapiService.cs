using System;

namespace EvolvingFunctionApp
{
    public class DapiService
    {
        private int _counter = 0;

        public string GetUniqueResponse()
        {
            _counter++;
            return $"DAPI_RESPONSE_{_counter}_{Guid.NewGuid()}";
        }
    }
}