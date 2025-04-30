using BE_Productos.Models;

namespace BE_Productos.Repository
{
    public interface IOrdersProductRepository
    {
        Task<OrdersProduct> AddOrdersProduct(OrdersProduct ordersProduct);
        Task<List<OrdersProduct>> GetListOrdersProduct();
        Task<OrdersProduct> GetOrdersProductr(int id);
        Task UpdateOrdersProduct(OrdersProduct ordersProduct);
        Task DeleteOrdersProduct(OrdersProduct ordersProduct);
    }
}
