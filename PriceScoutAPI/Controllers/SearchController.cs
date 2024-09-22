using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceScoutAPI.Helpers;
using PriceScoutAPI.Interfaces;
using PriceScoutAPI.Models;
using System.Globalization;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PriceScoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly AmazonHelper _amazonHelper;
        private readonly AliExpressHelper _aliExpressHelper;
        private readonly CurrencyHelper _currencyHelper;
        private readonly ILogger<SearchController> _logger;
        private readonly IBestOptionHelper _bestOptionHelper;

        public SearchController(
            AmazonHelper amazonHelper, 
            AliExpressHelper aliExpressHelper, 
            CurrencyHelper currencyHelper, 
            ILogger<SearchController> logger,
            IBestOptionHelper bestOptionHelper
        )
        {
            _bestOptionHelper = bestOptionHelper; 
            _aliExpressHelper = aliExpressHelper;
            _currencyHelper = currencyHelper;
            _amazonHelper = amazonHelper;
            _logger = logger;
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

                // -- If you activate the method, check if there are two values (Different from zero)
                // --- and check if the smaller one is greater than the larger one (this is a error!)
                bool bouthPricesValidateError = ((m.MinPrice != 0 && m.MaxPrice != 0) && m.MinPrice > m.MaxPrice);
                if(bouthPricesValidateError)
                {
                    resp.Success = false;
                    resp.Message = "Correct the values entered";
                    return BadRequest(resp);
                }

                // --- MIN or MAX prices
                if(priceSearchMethod &&  string.IsNullOrEmpty(m.Currency))
                {
                    return BadRequest("You need to pass the 'currency' property when you enter the minimum or maximum value");
                }

                //  --- Prepare the PRICE SEARCH METHOD
                if (priceSearchMethod)
                {

                    currencyPrice = await _currencyHelper.ChangeCurrency(m.Currency!.ToUpper());
                }

                // --- Country Parameter
                var countryParams = m.Country ?? "";
                var availableCountries = new string[] { "US", "BR", "ES", "FR", "NL", "CA", "IT", "PL", "AU", "DE", "BE" }.Contains(countryParams.ToUpper());
                var searchIn = availableCountries ? countryParams.ToUpper() : "US";
                m.Country = searchIn;


                // --- Handling the search parameter
                m.ProductName = m.ProductName.Replace(" ", "%20");

                /***************************************************************************************
                //  --- Search Prices
                /***************************************************************************************/
                var foundPrices = await SearchAllPrices(m, currencyPrice, m.Currency ?? "USD");
                if (foundPrices == null || foundPrices.Count == 0) return Ok();

                /***************************************************************************************
                //  --- Filtering Return Handle
                /***************************************************************************************/
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
                foundPrices = foundPrices.Take(limit).ToList();

                /***************************************************************************************
                //  --- Find the best option
                /***************************************************************************************/
                var bestOption = _bestOptionHelper.ChooseBestOption(foundPrices);


                /***************************************************************************************
                //  --- Prepare model for return
                /***************************************************************************************/
                var modelResp = new SearchModelResponse()
                {
                    BestProductOption = bestOption,
                    TotalProducts = foundPrices.Count,
                    LowestPrice = foundPrices.MinBy(x => x.Price).Price,
                    HighestPrice = foundPrices.MaxBy(x => x.Price).Price,
                    AveragePrice =  Math.Round(foundPrices.Average(x => x.Price),2),
                    Products = foundPrices
                };
                resp.Data = modelResp;

                return Ok(resp);


            }
            catch (Exception ex)
            {
                var logM = new LogModel
                {
                    Error = ex.ToString()[..150],
                    RequestModel = m,
                    ErrorCode = "ERR_SearchController_SearchSingleProduct"
                }; 
                
                _logger.LogError(JsonSerializer.Serialize(logM));
                resp.Success = false;
                resp.Message = "INTERNAL ERROR";
                return BadRequest(resp);
            }
            
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
                                Currency = "US",
                                Name = p.Item.Title,
                                IsBestSeller = false,
                                ECommerce = "AliExpress",
                                ID = p.Item.Id.ToString(),
                                ImageUrl = p.Item.ImageUrl,
                                StarRange = p.Item.StarRate ?? 0,
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
                                StarRange = double.Parse(p.StarRating, CultureInfo.InvariantCulture),
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
                var logM = new LogModel
                {
                    Error = ex.Message[..150],
                    RequestModel = model,
                    ErrorCode = "ERR03"
                };
                _logger.LogError(JsonSerializer.Serialize(logM));

            }

            return topList;
        }
    }
}
