using BE_Productos.Data;
using BE_Productos.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Productos.Repository
{
    public class OrdersProductRepository:IOrdersProductRepository
    {
        private readonly ApplicationDBContext _context;
        public OrdersProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task DeleteOrdersProduct(OrdersProduct ordersProduct)
        {
            var deleteOrdersProduct = await _context.OrdersProducts.FindAsync(ordersProduct.Id);
            if (deleteOrdersProduct != null)
            {
                deleteOrdersProduct.Active = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<OrdersProduct>> GetListOrdersProduct()
        {
            return await _context.OrdersProducts.Where(ordersProduct => ordersProduct.Active)
                .Include(ordersProduct => ordersProduct.Product)
                .ToListAsync();
        }

        public async Task<OrdersProduct> GetOrdersProductr(int id)
        {
            return await _context.OrdersProducts.Where(ordersProduct => ((ordersProduct.Id == id) && ordersProduct.Active))
                .Include(ordersProduct => ordersProduct.Product)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateOrdersProduct(OrdersProduct ordersProduct)
        {
            var updateOrdersProduct = await _context.OrdersProducts.FindAsync(ordersProduct.Id);
            if (updateOrdersProduct != null)
            {
                updateOrdersProduct.OrderId = ordersProduct.OrderId;
                updateOrdersProduct.ProductId = ordersProduct.ProductId;
                updateOrdersProduct.Quantity = ordersProduct.Quantity;
                await _context.SaveChangesAsync();
            }
        }
    }
}
