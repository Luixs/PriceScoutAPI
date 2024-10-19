using PriceScoutAPI.Models;

namespace PriceScoutAPI.Interfaces
{
    public interface IAliExpressHelper
    {
        public Task<AliExpressModel?> FindPrices(SearchModel m);
        public Task<AliExpressSingleModel?> FindUniqueProduct(string id);
    }
}
