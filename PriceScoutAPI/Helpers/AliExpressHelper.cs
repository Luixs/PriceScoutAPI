using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class AliExpressHelper
    {
        private readonly IConfiguration _configuration;
        private static HttpClient client = new HttpClient();
        private readonly ILogger<AliExpressHelper> _logger;

        public AliExpressHelper(IConfiguration configuration, ILogger<AliExpressHelper> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AliExpressModel?> FindPrices(SearchModel m)
        {
            var _key= _configuration["ApiKeys:RapidApi"];
            var _host = _configuration["ApiKeys:AliexpressHost"];

            try
            {
                var fullURL = String.Format("https://{0}/item_search?q={1}&page=1&sort=default", _host, m.ProductName); // -- For now on, params fixed's
                var requestM = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(fullURL),
                    Headers =
                    {
                        { "x-rapidapi-key", _key },
                        { "x-rapidapi-host", _host },
                    }
                };

                var response = await client.SendAsync(requestM);
                response.EnsureSuccessStatusCode();

                // --- RESPONSE BODY
                var DynamicBodyToFix = response.Content.ReadAsStringAsync().Result;

                var AllProducts = JsonSerializer.Deserialize<AliExpressModel>(DynamicBodyToFix);

                return AllProducts;

            }
            catch (Exception ex)
            {
                var logM = new LogModel
                {
                    Error = ex.Message[..150],
                    RequestModel = m,
                    ErrorCode = "ERR04"
                };

                _logger.LogError(JsonSerializer.Serialize(logM));

                return null;
            }
        }

    }
}
