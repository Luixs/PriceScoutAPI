using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class ProductModel // : BaseModel
    {
        [JsonPropertyName("productName")]
        public string Name { get; set; }
    }
}
