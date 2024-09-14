using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class CurrencyHelper
    {
        private readonly IConfiguration _configuration;
        private static HttpClient client = new HttpClient();


        public CurrencyHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<double> ChangeCurrency(string currencyParams)
        {
            try
            {
                // -- CHECK THE CURRENCY (DEFAULT IS 'USD')
                var isDefaultCurrency = currencyParams.Equals("USD");

                if (isDefaultCurrency) return 1.0; 
                else
                {

                    var _key = _configuration["ApiKeys:ExchangeRate"];
                    var fullURL = String.Format("https://v6.exchangerate-api.com/v6/{0}/latest/USD", _key); // -- For now on, params fixed's

                    var requestM = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(fullURL)
                    };

                    var response = await client.SendAsync(requestM);
                    response.EnsureSuccessStatusCode();

                    // --- RESPONSE BODY
                    var DynamicBody = response.Content.ReadAsStringAsync().Result;

                    var AllCurrency = JsonSerializer.Deserialize<CurrencyAPIModel>(DynamicBody);
                    if(AllCurrency == null) return 1.0;
                    
                    decimal selectedCurrency = AllCurrency.ConversionRates[currencyParams];

                    return double.Parse(selectedCurrency.ToString());
                }

            }
            catch (Exception ex)
            {
                return 1.0;
            }
            
        }
    }
}
