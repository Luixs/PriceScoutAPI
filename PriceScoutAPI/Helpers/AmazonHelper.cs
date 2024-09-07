using Microsoft.Extensions.Configuration;
using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class AmazonHelper
    {
        private readonly IConfiguration _configuration;
        private static HttpClient client = new HttpClient();

        public AmazonHelper(IConfiguration configuration) { 
            _configuration = configuration;
        }

        public async Task<string> FindPrices(SearchModel m)
        {
            var _host = _configuration["ApiKeys:Amazon:Host"];
            var _key =  _configuration["ApiKeys:Amazon:Key"];

            try
            {
                var fullURL = String.Format("https://{0}/search?query={1}&page=1&country=BR&sort_by=RELEVANCE&product_condition=ALL&is_prime=false", _host, m.ProductName); // -- For now on, params fixed's
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

                var AllProducts = JsonSerializer.Deserialize<AmazonModel>(DynamicBodyToFix);

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERR01 - AMZ");
                Console.WriteLine(ex.ToString().Substring(0, 500));
                Console.WriteLine("ERR01 - AMZ:END");
            }
            return "";
            
        }


    }
}
