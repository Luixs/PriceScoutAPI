using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class CurrencyAPIModel
    {

        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("base_code")]
        public string BaseCode { get; set; }

        [JsonPropertyName("conversion_rates")]
        public Dictionary<string, decimal> ConversionRates { get; set; }

        [JsonPropertyName("error-type")]
        public string ErrorType { get; set; }

    }
}
