using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class MercadoLibreModel
    {
        [JsonPropertyName("results")]
        public List<MercadoLibreProduct> Results { get; set; }
    }

    public class MercadoLibreProduct
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Name { get; set; }

        [JsonPropertyName("permalink")]
        public string Url { get; set; }

        [JsonPropertyName("reviews")]
        public MercadoLibreReview Reviews { get; set; }

        [JsonPropertyName("pictures")]
        public MercadoLibrePictures Pictures { get; set; }

        [JsonPropertyName("price")]
        public MercadoLibrePrice PriceInfo { get; set; }

        [JsonPropertyName("seller_info")]
        public MercadoLibreSellerInfo SelerInfo { get; set; }

    }

    public class MercadoLibreSellerInfo
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("power_seller_status")]
        public string SellerStatus { get; set; }

    }

    public class MercadoLibrePrice
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency_id")]
        public string Currency { get; set; }

        [JsonPropertyName("original_price")]
        public double OriginalPrice { get; set; }

    }


    public class MercadoLibrePictures
    {
        [JsonPropertyName("stack")]
        public MercadoLibreImgStack Images { get; set; }

    }

    public class MercadoLibreImgStack
    {
        [JsonPropertyName("retina")]
        public string Url { get; set; }
    }

    public class MercadoLibreReview
    {
        [JsonPropertyName("rating_average")]
        public double RatingAverage { get; set; }

        [JsonPropertyName("total")]
        public int ReviewCount { get; set; }

    }
    public class MercadoLibreSingleProductModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("share")]
        public MercadoLibreShare ShareInfos { get; set; }
    }

    public class MercadoLibreShare
    {
        [JsonPropertyName("permaLink")]
        public string Url { get; set; }

        [JsonPropertyName("title")]
        public string Name { get; set; }
    }
}
