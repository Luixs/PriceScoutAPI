using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class AmazonHelper
    {
        private readonly IConfiguration _configuration;
        private static HttpClient client = new HttpClient();

        public AmazonHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AmazonModel?> FindPrices(SearchModel m)
        {
            var _key = _configuration["ApiKeys:RapidApi"];
            var _host = _configuration["ApiKeys:AmazonHost"];

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

                // --- PEGANDO APENAS OS QUE TEM PREÇOS
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
                            p.PriceNumber = double.Parse(removeDolar.Trim());
                        }
                        catch (Exception ex)
                        {
                            // --- Log here, after...
                            p.PriceNumber = -9999.00;
                        }
                    }

                }

                return AllProducts;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERR01 - AMZ");
                Console.WriteLine(ex.ToString().Substring(0, 500));
                Console.WriteLine("ERR01 - AMZ:END");
                return null;
            }
        }


    }
}
