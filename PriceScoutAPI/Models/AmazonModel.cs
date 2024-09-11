using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class AmazonModel
    {
        [JsonPropertyName("status")]
        public string Status{ get; set; }

        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        [JsonPropertyName("data")]
        public InfoProductsAmazon Data { get; set; }
    }

    public class InfoProductsAmazon
    {
        [JsonPropertyName("total_products")]
        public int TotalProducts { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("products")]
        public List<ProductAmazon> Products { get; set; }
    }

    public class ProductAmazon
    {
        [JsonPropertyName("product_title")]
        public string Name{ get; set; }

        [JsonPropertyName("product_price")]
        public string Price { get; set; }

        [JsonPropertyName("product_price_number")]
        public double PriceNumber{ get; set; }

        [JsonPropertyName("product_original_price")]
        public string OriginalPrice { get; set; }

        [JsonPropertyName("product_url")]
        public string Url { get; set; }

        [JsonPropertyName("product_photo")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("is_best_seller")]
        public bool IsBestSeller { get; set; }

        [JsonPropertyName("is_amazon_choice")]
        public bool IsAmazonChoice { get; set; }

        [JsonPropertyName("is_prime")]
        public bool IsPrime { get; set; }

        [JsonPropertyName("sales_volume")]
        public string SalesVolume { get; set; }

        [JsonPropertyName("has_variations")]
        public bool HasVariations { get; set; }

        // ----- ^^^^^^^^ IMPORTANTES ACIMA

        [JsonPropertyName("product_star_rating")]
        public string StarRating { get; set; }

        [JsonPropertyName("product_num_ratings")]
        public int NumRatings { get; set; }

        [JsonPropertyName("asin")]
        public string Asin { get; set; }

        [JsonPropertyName("product_num_offers")]
        public int NumOffers { get; set; }

        [JsonPropertyName("product_minimum_offer_price")]
        public string MinOfferPrice { get; set; }

        [JsonPropertyName("climate_pledge_friendly")]
        public bool ClimateFriendly { get; set; }

        [JsonPropertyName("delivery")]
        public string Delivery { get; set; }

        
    }


}
