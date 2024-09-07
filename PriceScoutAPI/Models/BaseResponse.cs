using System.Collections.ObjectModel;
using System.Reflection;

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

        // <summary>
        /// Indicates whether the request was successful or not.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Request response message
        /// </summary>
        public string Message { get; set; }
    }
}
