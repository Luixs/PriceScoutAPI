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

        public SearchController(AmazonHelper amazonHelper, AliExpressHelper aliExpressHelper)
        {
            _amazonHelper = amazonHelper;
            _aliExpressHelper = aliExpressHelper;
        }

        [HttpPost]
        public async Task<IActionResult> SearchSingleProduct([FromBody] SearchModel model)
        {
            // --- INITAL VALUES
            var resp = new BaseResponse();

            try
            {
                // --- TRATAR AQUI OS PARÂMETROS DE ENTRADA
                model.ProductName = model.ProductName.Replace(" ", "%20");

                /**************************************************************************************
                //  --- Buscar preços
                /***************************************************************************************/
                var foundPrices = await SearchAllPrices(model);
                return Ok(foundPrices);



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
        private async Task<dynamic> SearchAllPrices(SearchModel model)
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
                if(aliExpressList != null && aliExpressList.Result.Products.Count > 0)
                {
                    foreach (var p in aliExpressList.Result.Products)
                    {
                        topList.Add(new ProductModel
                        {
                            ID = p.Item.Id.ToString(),
                            ECommerce = "AliExpress",
                            Currency = "US",
                            Name = p.Item.Title,
                            IsBestSeller = false,
                            ImageUrl = p.Item.ImageUrl,
                            Price = p.Item.Sku.Def.Prices.AppPrice
                        });
                    }
                }
                
                if(amazonList != null && amazonList.Data.TotalProducts > 0)
                {
                    foreach (var p in amazonList.Data.Products)
                    {
                        topList.Add(new ProductModel
                        {
                            ID = p.Asin,
                            ECommerce = "Amazon",
                            Currency = p.Currency,
                            Name = p.Name,
                            IsBestSeller = p.IsBestSeller,
                            ImageUrl = p.ImageUrl,
                            Price = p.PriceNumber,
                            Url = p.Url
                        });
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
