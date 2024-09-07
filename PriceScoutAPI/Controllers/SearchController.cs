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

        public SearchController(AmazonHelper amazonHelper)
        {
            _amazonHelper = amazonHelper;
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
            try
            {
                // --- TIMER HERE

                // --- AMAZON SEARCH
                var amazon = await _amazonHelper.FindPrices(model);

            }catch (Exception ex)
            {

            }

            return null;
        }
    }
}
