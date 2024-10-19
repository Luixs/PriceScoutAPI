using PriceScoutAPI.Interfaces;
using PriceScoutAPI.Models;
using System.Globalization;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class AmazonHelper : IAmazonHelper
    {
        private HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AmazonHelper> _logger;
        private readonly string _key;
        private readonly string _host;

        public AmazonHelper(IConfiguration configuration, ILogger<AmazonHelper> logger, IHttpClientFactory client)
        {
            _configuration = configuration;
            _logger = logger;
            _client = client.CreateClient(nameof(IAmazonHelper));

            // --- Getting consts...
            _key = _configuration["ApiKeys:RapidApi"] ?? "";
            _host = _configuration["ApiKeys:AmazonHost"] ?? "";
        }

        public async Task<AmazonModel?> FindPrices(SearchModel m)
        {
            // --- Initial values
            var filters = "";

            // --- Has a minimum price?
            if(m.MinPrice != 0)
            {
                filters = $"&min_price={m.MinPrice}";
            }

            // --- Has a maximum price?
            if (m.MaxPrice != 0)
            {
                filters = $"{filters}&max_price={m.MaxPrice}";
            }

            try
            {
                var fullURL = String.Format("https://{0}/search?query={1}&page=1&country={2}&sort_by=RELEVANCE&product_condition=ALL&is_prime=false", _host, m.ProductName, m.Country); // -- For now on, params fixed's
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



                var AllProducts = JsonSerializer.Deserialize<AmazonModel>(DynamicBodyToFix);

                // --- Get only if exist array
                if (AllProducts != null && AllProducts.Data.Products.Count > 0)
                {

                    foreach (var p in AllProducts.Data.Products)
                    {
                        var removeDolar =  
                            !string.IsNullOrWhiteSpace(p.Price) ? // -- If exist PRICE
                            // --- Try to Replace Brazilian Currency (R$). If not, convert Dolar ($)
                            (p.Price.Contains("R$") ? p.Price.Replace("R$", "") : p.Price.Replace("$", ""))
                            // -- If not exist PRICE, get the Original Price
                            : !string.IsNullOrWhiteSpace(p.OriginalPrice)
                            // --- Try to Replace Brazilian Currency (R$). If not, convert Dolar ($)
                            ? (p.OriginalPrice.Contains("R$") ? p.OriginalPrice.Replace("R$", "") : p.OriginalPrice.Replace("$", "")) : "0";


                        try
                        {
                            p.PriceNumber = double.Parse(removeDolar.Trim(),CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            p.PriceNumber = -9999.00;
                        }
                    }

                }

                return AllProducts;
            }
            catch (Exception ex)
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

        public async Task<AmazonSingleModel?> FindUniqueProduct(string id)
        {
            try
            {
                var fullURL = String.Format("https://{0}/product-details?asin={1}", _host, id); // -- For now on, params fixed's
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

                // --- HANDLING THE RESPONSE BODY
                var DynamicBodyToFix = response.Content.ReadAsStringAsync().Result;
                var ProductFound = JsonSerializer.Deserialize<AmazonSingleModel>(DynamicBodyToFix);

                return ProductFound;


            } catch (Exception ex)
            {
                var logM = new LogModel
                {
                    Error = ex.Message.Substring(0, 150),
                    RequestModel = id,
                    ErrorCode = "ERR03"
                };

                _logger.LogError(JsonSerializer.Serialize(logM));
                return null;
            }
        }
    }
}
