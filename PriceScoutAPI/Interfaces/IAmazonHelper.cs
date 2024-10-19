using PriceScoutAPI.Models;

namespace PriceScoutAPI.Interfaces
{
    public interface IAmazonHelper
    {
        public Task<AmazonModel?> FindPrices(SearchModel m);
        public Task<AmazonSingleModel?> FindUniqueProduct(string id);
    }
}
