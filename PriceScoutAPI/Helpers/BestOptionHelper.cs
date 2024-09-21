using PriceScoutAPI.Controllers;
using PriceScoutAPI.Interfaces;
using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Helpers
{
    public class BestOptionHelper : IBestOptionHelper
    {
        private readonly ILogger<BestOptionHelper> _logger;

        public BestOptionHelper(ILogger<BestOptionHelper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Search for the best product option according to the mapped criteria.
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public ProductModel ChooseBestOption(List<ProductModel> products)
        {

            var scoreProducts = products.OrderByDescending(p => CalculateProductScore(p)).ToArray();

            // --- Normalizing values
            return scoreProducts[0];
        }

        /// <summary>
        /// Calculation Mechanism to find the product score
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private double CalculateProductScore(ProductModel p)
        {
            try
            {
                // --- Define weight for each attribute
                double weightPrice = 0.5;
                double weightStarRange = 0.4;
                double weightBestSelleer = 0.3;
                double weightEcommerce = 0.1;

                // --- Normalizing values
                double normalizedPrice = 1 / p.Price; // -- The lower price, more score
                double normalizedStars = p.StarRange / 5;
                double normalizedBestSeller = p.IsBestSeller ? 1 : 0;
                double normalizedEcommerce = 1; // -- All ecommerce has the same

                // --- Calculating the Score
                double finalScore = 
                    (normalizedPrice * weightPrice) + 
                    (normalizedStars * weightStarRange) + 
                    (normalizedBestSeller * weightBestSelleer) + 
                    (normalizedEcommerce * weightEcommerce);

                return finalScore;

            }catch (Exception ex)
            {

                _logger.LogError(JsonSerializer.Serialize(new LogModel{
                    Error = ex.ToString()[..150],
                    RequestModel = p,
                    ErrorCode = "ERR_BestOptionHelper_CalculateProduct"
                }));

                return 0.0;
            }
        }
        
    }
}
