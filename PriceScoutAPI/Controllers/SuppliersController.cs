using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceScoutAPI.Models;

namespace PriceScoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        /// <summary>
        /// Return all suplliers that we have!
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            List<string> sups = new List<string>(){ "Amazon","AliExpress","Mercado Libre" };
            
            var resp = new BaseResponse("Seeing All Suppliers!", sups);

            return Ok(resp);
        }
    }
}
