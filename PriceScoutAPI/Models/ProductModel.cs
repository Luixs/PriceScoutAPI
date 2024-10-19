using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class ProductModel : BaseModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("e_commerce")]
        public string ECommerce { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("is_best_seller")]
        public bool IsBestSeller { get; set; }

        [JsonPropertyName("star_range")]
        public double StarRange { get; set; }
    }

    public class ProductModelResponse
    {

        [JsonPropertyName("total_itens_found")]
        public int TotalProducts { get; set; }

        [JsonPropertyName("product")]
        public List<dynamic> Products { get; set; }

    }

}
