using System.Transactions;
using AutoMapper;
using BE_Productos.Data;
using BE_Productos.Models;
using BE_Productos.Models.DTO;
using BE_Productos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;
        public OrderController (IOrderRepository orderRepository,
            IProductRepository productRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            ApplicationDBContext context)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _orderRepository.GetListOrders();
                var ordersDto = _mapper.Map<List<OrderGetDTO>>(orders);
                return Ok(ordersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrder(id);
                if (order == null)
                {
                    return NotFound($"There is no order with the ID: {id}");
                }
                var orderDto = _mapper.Map<OrderGetDTO>(order);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet("Employees-Products")]
        public async Task<IActionResult> GetEmployeesProducts()
        {
            try
            {
                var employees = await _employeeRepository.GetListEmployees();
                var products = await _productRepository.GetListProducts();
                var employeesDto = _mapper.Map<List<EmployeeDTO>>(employees);
                var productsDto = _mapper.Map<List<ProductDTO>>(products);
                var employeesProducts = new
                {
                    Employees = employeesDto,
                    Products = productsDto
                };
                return Ok(employeesProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderCreateDTO orderDto)
        {
            /* 
             * Entity Framework maneja las transacciones con IDbContextTransaction que tiene IDisposable por lo que se recomienda el uso
             * de using para que se liberen los recursos al finalizar la transacción y evitar fugas de memoria
             */
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if(orderDto.Total <= 0)
                {
                    return BadRequest("Some fields have incorrect values");
                }

                foreach (var product in orderDto.OrdersProducts)
                {
                    if (product.Quantity <= 0)
                    {
                        return BadRequest("Some products have incorrect quantities");
                    }
                }
                var order = new Order
                {
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    EmployeeId = orderDto.EmployeeId,
                    Total = orderDto.Total,
                    Active = true,
                    // Creamos una lista de objetos OrdersProduct a partir de la lista de productos del DTO con linq
                    OrdersProducts = orderDto.OrdersProducts.Select(orderProduct => new OrdersProduct
                    {
                        ProductId = orderProduct.ProductId,
                        Quantity = orderProduct.Quantity,
                        Active = true
                    }).ToList()
                };

                /* 
                 * Como se creo el objeto de order , ahora se le asigna OrderId a cada objeto OrdersProduct por parte de EntityFramework
                 * ya que se relaciona automaticamente con el objeto Order, primero se guarda el objeto Order y luego se guardan los OrdersProduct
                 * Por lo que iniciamos una transacción para guardar el objeto Order y luego los OrdersProduct y si hay un error se hace rollback
                 */
                var newOrder = await _orderRepository.AddOrder(order);
                await transaction.CommitAsync();

                // Hacemos un select para obtner el objeto Order completo con los OrdersProduct y el Employee
                var completeOrder = await _orderRepository.GetOrder(newOrder.Id);
                var neWOrderDto = _mapper.Map<OrderGetDTO>(completeOrder);
                return CreatedAtAction("GetOrder", new { id = neWOrderDto.Id }, neWOrderDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrder(id);
                if (order == null)
                {
                    return NotFound($"There is no order with the ID: {id}");
                }
                await _orderRepository.DeleteOrder(order);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
