using BE_Productos.Models;

namespace BE_Productos.Repository
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategory(Category category);
        Task<List<Category>> GetListCategories();
        Task<Category> GetCategory(int id);
        Task UpdateCategory(Category category);
        Task DeleteCategory(Category category);
    }
}
