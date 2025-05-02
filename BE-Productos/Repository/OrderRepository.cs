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
                .Include(order => order.OrdersProducts)
                .ThenInclude(ordersProduct => ordersProduct.Product)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            var updateOrder = await _context.Orders.FindAsync(order.Id);
            if (updateOrder != null)
            {
                updateOrder.EmployeeId = order.EmployeeId;
                updateOrder.Total = order.Total;
                await _context.SaveChangesAsync();
            }
        }
    }
}
