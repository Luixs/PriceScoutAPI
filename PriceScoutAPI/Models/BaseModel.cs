using System.Runtime.Serialization;

namespace PriceScoutAPI.Models
{
    public class BaseModel
    {
        [DataMember]
        public string ID { get; internal set; }
    }
}
