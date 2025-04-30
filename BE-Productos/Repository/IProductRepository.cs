using BE_Productos.Models;

namespace BE_Productos.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddProducts(Product product);
        Task<List<Product>> GetListProducts();
        Task<Product> GetProduct(int id);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);
    }
}
