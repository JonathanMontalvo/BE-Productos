using BE_Productos.Data;
using BE_Productos.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Productos.Repository
{
    public class OrderRepository: IOrderRepository
    {
        private readonly ApplicationDBContext _context;
        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Order> AddOrder(Order order)
        {
            /*
             * Entity Framework inicia una transacción automáticamente al agregar un nuevo registro y cuando se llama a SaveChangesAsync
             * realiza el commit de la transacción, si no se llama a SaveChangesAsync no se guardan los cambios en la base de datos
             * Nos evita generar unna transacción manualmente
             */
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrder(Order order)
        {
            var deleteOrder = await _context.Orders
                .Include(ordersProducts => ordersProducts.OrdersProducts) //Join con Orders_Products
                .FirstOrDefaultAsync(o => o.Id == order.Id && o.Active);

            if (deleteOrder != null)
            {
                deleteOrder.Active = false;

                // Se desactivan lógicamente los OrdersProducts de ese order
                foreach (var ordersProduct in deleteOrder.OrdersProducts)
                {
                    ordersProduct.Active = false;
                }

                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<Order>> GetListOrders()
        {
            return await _context.Orders.Where(order => order.Active == true)
                .Include(order => order.Employee) // Join con Employee
                .Include(order => order.OrdersProducts) // Join con Orders_Products
                .ThenInclude(ordersProduct => ordersProduct.Product) // Join con Products por parte de Orders_Products
                .ToListAsync();
        }

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders.Where(order => ((order.Id == id) && order.Active))
                .Include(order => order.Employee)
                .Include(order => order.OrdersProducts.Where(op => op.Active))
                .ThenInclude(ordersProduct => ordersProduct.Product)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            var updateOrder = await _context.Orders.Where(or => ((or.Id == order.Id) && or.Active))
                .Include(o => o.OrdersProducts.Where(op => op.Active)) // Incluye la lista de productos relacionados
                .FirstOrDefaultAsync();
            if (updateOrder != null)
            {
                Console.WriteLine("PAse el if\n");
                updateOrder.EmployeeId = order.EmployeeId;
                updateOrder.Total = order.Total;

                // Actualizar lista de OrdersProducts
                foreach (var existingOrderProduct in updateOrder.OrdersProducts)
                {
                    var updatedProduct = order.OrdersProducts.FirstOrDefault(op => op.ProductId == existingOrderProduct.ProductId);
                    if (updatedProduct != null)
                    {
                        existingOrderProduct.Quantity = updatedProduct.Quantity;
                        existingOrderProduct.Active = updatedProduct.Active;
                    }
                    else
                    {
                        existingOrderProduct.Active = false; // Marcar como inactivo si no está en la nueva lista
                    }
                }

                foreach (var newOrderProduct in order.OrdersProducts)
                {
                    if (!updateOrder.OrdersProducts.Any(op => op.ProductId == newOrderProduct.ProductId))
                    {
                        updateOrder.OrdersProducts.Add(new OrdersProduct
                        {
                            ProductId = newOrderProduct.ProductId,
                            Quantity = newOrderProduct.Quantity,
                            Active = true
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
