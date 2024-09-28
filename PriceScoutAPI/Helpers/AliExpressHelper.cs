using PriceScoutAPI.Interfaces;
using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class AliExpressHelper : IAliExpressHelper
    {
        private readonly IConfiguration _configuration;
        private HttpClient _client;
        private readonly ILogger<AliExpressHelper> _logger;

        public AliExpressHelper(IConfiguration configuration, ILogger<AliExpressHelper> logger, IHttpClientFactory client)
        {
            _configuration = configuration;
            _logger = logger;
            _client = client.CreateClient(nameof(IAliExpressHelper));
        }

        public async Task<AliExpressModel?> FindPrices(SearchModel m)
        {
            var _key= _configuration["ApiKeys:RapidApi"];
            var _host = _configuration["ApiKeys:AliexpressHost"];
            var filters = "";

            // --- Has a minimum price?
            if (m.MinPrice != 0)
            {
                filters = $"&startPrice={m.MinPrice}";
            }

            // --- Has a maximum price?
            if (m.MaxPrice != 0)
            {
                filters = $"{filters}&endPrice={m.MaxPrice}";
            }

            try
            {
                var fullURL = String.Format("https://{0}/item_search?q={1}&page=1&sort=default&region={2}", _host, m.ProductName, m.Country); // -- For now on, params fixed's
                fullURL += filters;
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

                var response = await _client.SendAsync(requestM);
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
