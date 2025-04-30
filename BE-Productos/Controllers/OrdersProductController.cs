using BE_Productos.Models;
using BE_Productos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersProductController : ControllerBase
    {
        private readonly IOrdersProductRepository _ordersProductRepository;

        public OrdersProductController(IOrdersProductRepository ordersProductRepository)
        {
            _ordersProductRepository = ordersProductRepository;
        }

        /*
        [HttpGet ("list")]
        public async Task<IActionResult> ListOrdersProducts()
        {
            try
            {
                var listOrdersProducts = await _ordersProductRepository.GetListOrdersProduct();

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }*/
    }
}
