using PriceScoutAPI.Interfaces;
using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class MercadoLibreHelper : IMercadoLibreHelper
    {
        private readonly ILogger<MercadoLibreHelper> _logger;
        private readonly IConfiguration _configuration;
        private HttpClient _client;
        private readonly string _key;
        private readonly string _host;

        public MercadoLibreHelper(IConfiguration configuration, ILogger<MercadoLibreHelper> logger, IHttpClientFactory client)
        {
            _configuration = configuration;
            _logger = logger;
            _client = client.CreateClient(nameof(IMercadoLibreHelper));

            // --- Getting consts...
            _key = _configuration["ApiKeys:RapidApi"] ?? "";
            _host = _configuration["ApiKeys:MercadoLibreHost"] ?? "";
        }

        public async Task<MercadoLibreModel> FindPrices(SearchModel m)
        {
            try
            {
                var fullURL = String.Format("https://{0}/search?coutry={1}&search={2}", _host, m.Country, m.ProductName);

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
                var AllProducts = JsonSerializer.Deserialize<MercadoLibreModel>(DynamicBodyToFix);

                return AllProducts;
            } catch (Exception ex)
            {

                var logM = new LogModel
                {
                    Error = ex.Message.Substring(0, 150),
                    RequestModel = m,
                    ErrorCode = "ERR03"
                };

                _logger.LogError(JsonSerializer.Serialize(logM));
                return null;
            }
        }
    }
}
