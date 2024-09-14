using PriceScoutAPI.Helpers.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class SearchModel
    {
        [Required(ErrorMessage ="The 'productName' property is required! Please, see the documentation")]
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        [StringLength(3, ErrorMessage = "The 'currency' property must contain exacly {1} character", MinimumLength = 3)]
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("minPrice")]
        public double MinPrice { get; set; }

        [JsonPropertyName("maxPrice")]
        public double MaxPrice { get; set; }

        [JsonPropertyName("sortBy")]
        [SortOrderValidator]
        public string? SortBy { get; set; }

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        //[JsonPropertyName("barCode")]
        //public string BarCode { get; set; }



        //[JsonPropertyName("store")]
        //public string Store { get; set; }

        //[JsonPropertyName("category")]
        //public string Category { get; set; }

    }
}
