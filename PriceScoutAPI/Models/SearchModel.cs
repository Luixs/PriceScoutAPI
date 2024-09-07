using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class SearchModel
    {
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        //[JsonPropertyName("barCode")]
        //public string BarCode { get; set; }

        //[JsonPropertyName("currency")]
        //public string Currency { get; set; }

        //[JsonPropertyName("sortBy")]
        //public string SortBy { get; set; }

        //[JsonPropertyName("limit")]
        //public int Limit { get; set; }

        //[JsonPropertyName("store")]
        //public string Store { get; set; }

        //[JsonPropertyName("category")]
        //public string Category { get; set; }

    }
}
