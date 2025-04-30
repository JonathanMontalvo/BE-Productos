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

        public Task<OrdersProduct> AddOrdersProduct(OrdersProduct ordersProduct)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrdersProduct(OrdersProduct ordersProduct)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrdersProduct>> GetListOrdersProduct()
        {
            throw new NotImplementedException();
        }

        public Task<OrdersProduct> GetOrdersProductr(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrdersProduct(OrdersProduct ordersProduct)
        {
            throw new NotImplementedException();
        }
    }
}
