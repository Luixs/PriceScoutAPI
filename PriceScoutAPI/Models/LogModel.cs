using PriceScoutAPI.Controllers;
using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class LogModel
    {
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("requestModel")]
        public dynamic RequestModel { get; set; }
    }
}
