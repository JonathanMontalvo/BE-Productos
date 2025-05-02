using BE_Productos.Data;
using BE_Productos.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Productos.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDBContext _context;
        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Category> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategory(Category category)
        {
            var deleteCategory = await _context.Categories.FindAsync(category.Id);
            if (deleteCategory != null)
            {
                deleteCategory.Active = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.Where(Category => ((Category.Id == id) && Category.Active)).FirstOrDefaultAsync();
        }

        public async Task<List<Category>> GetListCategories()
        {
            return await _context.Categories.Where(category => category.Active).ToListAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            var updateCategory = await _context.Categories.FindAsync(category.Id);

            if (UpdateCategory != null)
            {
                var existingCategory = await _context.Categories.Where(c => c.Name == category.Name && c.Active).FirstOrDefaultAsync();
                if (existingCategory != null)
                {
                    throw new Exception($"The name '{category.Name}' is already in use by another category");
                }
                updateCategory.Name = category.Name;
                await _context.SaveChangesAsync();
            }
        }
    }
}
