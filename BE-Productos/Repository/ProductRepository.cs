using BE_Productos.Data;
using BE_Productos.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Productos.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Product> AddProducts(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProduct(Product product)
        {
            var deleteProduct = await _context.Products.FindAsync(product.Id);
            if (deleteProduct != null)
            {
                deleteProduct.Active = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetListProducts()
        {
            return await _context.Products.Where(product => product.Active)
                .Include(product => product.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.Where(product => ((product.Id == id) && product.Active))
                .Include(product => product.Category)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            var updateProduct = await _context.Products.FindAsync(product.Id);
            if (updateProduct != null)
            {
                updateProduct.Name = product.Name;
                updateProduct.Price = product.Price;
                updateProduct.CategoryId = product.CategoryId;
                await _context.SaveChangesAsync();
            }
        }
    }
}
