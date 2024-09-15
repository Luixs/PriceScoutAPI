using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class AliExpressModel
    {
        [JsonPropertyName("result")]
        public InfoAliExpressProducts Result { get; set; }
    }

    public class InfoAliExpressProducts
    {
        [JsonPropertyName("status")]
        public StatusAli Status { get; set; }

        [JsonPropertyName("resultList")]
        public List<AliExpressProduct> Products { get; set; }
    }

    public class AliExpressProduct
    {
        [JsonPropertyName("item")]
        public ItemAliExpress Item { get; set; }
    }

    public class ItemAliExpress
    {
        [JsonPropertyName("itemId")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("sales")]
        public int Sales{ get; set; }

        [JsonPropertyName("image")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("sku")]
        public ItemAliSku Sku { get; set; }

        [JsonPropertyName("averageStarRate")]
        public double? StarRate { get; set; }
    }

    public class ItemAliSku
    {
        [JsonPropertyName("def")]
        public ItemAliSkuDef Def { get; set; }
    }

    public class ItemAliSkuDef
    {
        [JsonPropertyName("prices")]
        public ItemAliSkuDefPrices Prices { get; set; }
    }

    public class ItemAliSkuDefPrices
    {
        [JsonPropertyName("pc")]
        public double PcPrice { get; set; }

        [JsonPropertyName("app")]
        public double AppPrice { get; set; }
    }

    public class StatusAli
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }

    }
}
