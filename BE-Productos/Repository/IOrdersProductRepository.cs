using BE_Productos.Models;

namespace BE_Productos.Repository
{
    public interface IOrdersProductRepository
    {
        Task<List<OrdersProduct>> GetListOrdersProduct();
        Task<List<OrdersProduct>> GetOrdersProduct(int id);
        Task UpdateOrdersProduct(OrdersProduct ordersProduct);
        Task DeleteOrdersProduct(OrdersProduct ordersProduct);
    }
}
