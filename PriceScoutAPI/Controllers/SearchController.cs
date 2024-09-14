using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceScoutAPI.Helpers;
using PriceScoutAPI.Models;

namespace PriceScoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly AmazonHelper _amazonHelper;
        private readonly AliExpressHelper _aliExpressHelper;
        private readonly CurrencyHelper _currencyHelper;

        public SearchController(AmazonHelper amazonHelper, AliExpressHelper aliExpressHelper, CurrencyHelper currencyHelper)
        {
            _amazonHelper = amazonHelper;
            _aliExpressHelper = aliExpressHelper;
            _currencyHelper = currencyHelper;
        }

        [HttpPost]
        public async Task<IActionResult> SearchSingleProduct([FromBody] SearchModel m)
        {
            // --- INITAL VALUES
            var resp = new BaseResponse();
            var currencyPrice = 1.0;

            try
            {
                /**************************************************************************************
                //  --- Check the received Params
                /***************************************************************************************/
                bool priceSearchMethod = (m.MinPrice != 0 || m.MaxPrice != 0);

                // --- MIN or MAX prices
                if(priceSearchMethod &&  string.IsNullOrEmpty(m.Currency))
                {
                    return BadRequest("You need to pass the 'currency' property when you enter the minimum or maximum value");
                }

                /**************************************************************************************
                //  --- Prepare the PRICE SEARCH METHOD
                /**************************************************************************************/
                if (priceSearchMethod)
                {
                    currencyPrice = await _currencyHelper.ChangeCurrency(m.Currency!.ToUpper());
                }


                // --- Handling the search parameter
                m.ProductName = m.ProductName.Replace(" ", "%20");

                /***************************************************************************************
                //  --- Search Prices
                /***************************************************************************************/
                var foundPrices = await SearchAllPrices(m, currencyPrice, m.Currency ?? "USD");


                /***************************************************************************************
                //  --- Filtering Return Handle
                /***************************************************************************************/
                if (priceSearchMethod && m.MinPrice != 0) // --- Min Price
                {
                    foundPrices = foundPrices.FindAll(x=> x.Price >= m.MinPrice);
                }
                if(priceSearchMethod && m.MaxPrice > 0) // --- Max Price
                {
                    foundPrices = foundPrices.FindAll(x => x.Price <= m.MaxPrice);
                }

                // --- SORT BY (DESC | ASC)
                var sortBy = m.SortBy ?? "ASC";
                switch (sortBy.ToUpper())
                {
                    case "DESC":
                        foundPrices = foundPrices.OrderByDescending(x => x.Price).ToList();
                    break;
                }

                // --- LIMIT 
                var limit = m.Limit ?? foundPrices.Count;
                return Ok(foundPrices.Take(limit).ToList());


            }
            catch (Exception ex)
            {

            }
            return Ok(resp);
        }

        /**************************************************************************************
        // @LuisStarlino |  07/09/2024  10"00
        //  --- Method responsible for finding the price of a product within the available 
        //      microservices.
        /***************************************************************************************/
        private async Task<List<ProductModel>> SearchAllPrices(SearchModel model, double currencyMultiply, string currencySelected)
        {
            var topList = new List<ProductModel>();

            try
            {
                // --- TIMER HERE

                // --- AMAZON SEARCH
                var amazonList = await _amazonHelper.FindPrices(model);

                // --- ALIEXPRESS SEARCH
                var aliExpressList = await _aliExpressHelper.FindPrices(model);

                /**************************************************************************************
                //  --- Listando de acordo com os melhores atributos selecionados ou disponibilizados
                //      pela plataforma.
                /***************************************************************************************/
                if (aliExpressList != null && aliExpressList.Result.Products != null && aliExpressList.Result.Products.Count > 0)
                {
                    foreach (var p in aliExpressList.Result.Products)
                    {
                        if(p.Item.Sku.Def.Prices.AppPrice != 0)
                        {
                            topList.Add(new ProductModel
                            {
                                ID = p.Item.Id.ToString(),
                                ECommerce = "AliExpress",
                                Currency = "US",
                                Name = p.Item.Title,
                                IsBestSeller = false,
                                ImageUrl = p.Item.ImageUrl,
                                Price = p.Item.Sku.Def.Prices.AppPrice * currencyMultiply
                            });
                        }
                    }
                }
                
                if(amazonList != null && amazonList.Data.TotalProducts > 0 && amazonList.Data.Products != null)
                {
                    foreach (var p in amazonList.Data.Products)
                    {
                        if(p.PriceNumber != 0)
                        {
                            topList.Add(new ProductModel
                            {
                                ID = p.Asin,
                                ECommerce = "Amazon",
                                Currency = p.Currency ?? "",
                                Name = p.Name,
                                IsBestSeller = p.IsBestSeller,
                                ImageUrl = p.ImageUrl,
                                Price = (p.Currency != null && p.Currency.ToUpper().Equals(currencySelected)) ? p.PriceNumber : p.PriceNumber * currencyMultiply,
                                Url = p.Url
                            });
                        }
                    }
                }

                return topList.OrderBy(x => x.Price).ToList();

            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
