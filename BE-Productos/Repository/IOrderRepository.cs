using BE_Productos.Models;

namespace BE_Productos.Repository
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order order);
        Task<Order> GetOrder(int id);
        Task<List<Order>> GetListOrders();
        Task UpdateOrder(Order order);
        Task DeleteOrder(Order order);
    }
}
