using PriceScoutAPI.Models;

namespace PriceScoutAPI.Interfaces
{
    public interface IBestOptionHelper
    {
        public ProductModel ChooseBestOption(List<ProductModel> products);
    }
}
