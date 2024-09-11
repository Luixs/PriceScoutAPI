using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class AliExpressHelper
    {
        private readonly IConfiguration _configuration;
        private static HttpClient client = new HttpClient();

        public AliExpressHelper(IConfiguration configuration)
        {
            _configuration = configuration;
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
                Console.WriteLine("ERR01 - AliExpress");
                Console.WriteLine(ex.ToString().Substring(0, 500));
                Console.WriteLine("ERR01 - AliExpress:END");
                return null;
            }
        }

    }
}
