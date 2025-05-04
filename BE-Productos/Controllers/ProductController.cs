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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> ListProducts()
        {
            try
            {
                var products = await _productRepository.GetListProducts();
                var productsDto = _mapper.Map<List<ProductGetDTO>>(products);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);
                var productDto = _mapper.Map<ProductGetDTO>(product);
                if (product == null)
                {
                    return NotFound($"There is no product with the ID: {id}");
                }
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO productDto)
        {
            try
            {
                if (productDto.Name == null || productDto.Price <= 0)
                {
                    return BadRequest("Some fields have incorrect values");
                }
                var product = _mapper.Map<Product>(productDto);
                product.Active = true;


                var newProduct = await _productRepository.AddProducts(product);
                var newProductDto = _mapper.Map<ProductDTO>(newProduct);
                return CreatedAtAction("GetProduct", new { id = newProductDto.Id }, newProductDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDto)
        {
            try
            {
                if (id != productDto.Id || productDto.Name == null || productDto.Price <= 0)
                {
                    return BadRequest("Some fields have incorrect values");
                }

                var existingProduct = await _productRepository.GetProduct(id);
                if (existingProduct == null)
                {
                    return NotFound($"There is no product with the ID: {id}");
                }
                if (existingProduct.Active == false)
                {
                    return BadRequest($"The product with ID: {id} is inactive");
                }
                var product = _mapper.Map<Product>(productDto);
                await _productRepository.UpdateProduct(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);
                if (product == null || product.Active == false)
                {
                    return NotFound($"There is no product with the ID: {id}");
                }
                await _productRepository.DeleteProduct(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
