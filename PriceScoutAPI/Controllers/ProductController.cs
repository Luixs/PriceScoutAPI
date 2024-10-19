using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceScoutAPI.Interfaces;
using PriceScoutAPI.Models;
using System.Text.Json;

namespace PriceScoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IAmazonHelper _amazonHelper;
        private readonly IAliExpressHelper _aliExpressHelper;
        private readonly IMercadoLibreHelper _mercadoLibreHelper;

        public ProductController (
            ILogger<ProductController> logger,
            IAmazonHelper amazonHelper,
            IAliExpressHelper aliExpressHelper
        )
        {
            _logger = logger;
            _amazonHelper = amazonHelper;
            _aliExpressHelper = aliExpressHelper;
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetProductDetails(string id)
        {
            // --- INITAL VALUES
            var resp = new BaseResponse();

            try
            {
                /**************************************************************************************
                //  --- Get products information
                /***************************************************************************************/
                var productResp = await FindProductInSuppliers(id);

                resp.Data = new ProductModelResponse
                {
                    TotalProducts = productResp.Count,
                    Products = productResp
                };

            }
            catch (Exception ex)
            {
                var logM = new LogModel
                {
                    Error = ex.ToString()[..150],
                    RequestModel = id,
                    ErrorCode = "ERR_ProductController_ListProductDetails"
                };

                _logger.LogError(JsonSerializer.Serialize(logM));
                resp.Success = false;
                resp.Message = "INTERNAL ERROR";
                return BadRequest(resp);
            }

             return Ok(resp);
        }

        /**************************************************************************************
        // @LuisStarlino |  07/09/2024  11"11
        //  --- Method responsible for finding the details of a single product based on 
        //      id.
        /***************************************************************************************/
        private async Task<List<dynamic>> FindProductInSuppliers(string id)
        {
            var dynamicReturnList = new List<dynamic>();
            try
            {
                // --- TRY TO CATCH PRODUCT ON AMAZON (B08PPBQM23)
                var amazonP = await _amazonHelper.FindUniqueProduct(id);
                if (amazonP != null  && !string.IsNullOrEmpty(amazonP.Data.Name)) dynamicReturnList.Add(amazonP);
         

                // --- TRY TO CATCH PRODUCT ON ALI EXPRESS
                var aliExpressP = await _aliExpressHelper.FindUniqueProduct(id);
                if (aliExpressP.Result.Item != null) dynamicReturnList.Add(aliExpressP);
                // --- Putting on the current returns model

                return dynamicReturnList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
