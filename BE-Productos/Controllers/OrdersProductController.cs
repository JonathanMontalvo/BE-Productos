using AutoMapper;
using BE_Productos.Models;
using BE_Productos.Models.DTO;
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
        private readonly IMapper _mapper;

        public OrdersProductController(IOrdersProductRepository ordersProductRepository, IMapper mapper)
        {
            _ordersProductRepository = ordersProductRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersProducts()
        {
            try
            {
                var ordersProducts = await _ordersProductRepository.GetListOrdersProduct();
                var ordersProductsDto = _mapper.Map<List<OrdersProductGetDTO>>(ordersProducts);
                return Ok(ordersProductsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersProduct(int id)
        {
            try
            {
                var ordersProduct = await _ordersProductRepository.GetOrdersProductr(id);
                if (ordersProduct == null)
                {
                    return NotFound($"There is no order product with the ID: {id}");
                }
                var ordersProductDto = _mapper.Map<OrdersProductGetDTO>(ordersProduct);
                return Ok(ordersProductDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
