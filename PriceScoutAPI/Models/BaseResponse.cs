using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json.Serialization;

namespace PriceScoutAPI.Models
{
    public class BaseResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseResponse()
        {
            Success = true;
            Message = "OK";
        }

        public BaseResponse(string m, dynamic d)
        {
            this.Success = true;
            this.Message = m;
            this.Data = d;
        }

        // <summary>
        /// Indicates whether the request was successful or not.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Request response message
        /// </summary>
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public dynamic Data { get; set; }
    }
}
