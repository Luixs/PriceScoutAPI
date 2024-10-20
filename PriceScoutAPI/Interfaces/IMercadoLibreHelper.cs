using PriceScoutAPI.Models;

namespace PriceScoutAPI.Interfaces
{
    public interface IMercadoLibreHelper
    {
        public Task<MercadoLibreModel?> FindPrices(SearchModel m);
        public Task<MercadoLibreSingleProductModel?> FindUniqueProduct(string id);
    }
}
