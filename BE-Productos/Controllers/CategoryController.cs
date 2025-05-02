using AutoMapper;
using BE_Productos.Models;
using BE_Productos.Models.DTO;
using BE_Productos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper) {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetListCategories();
                var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories);

                return Ok(categoriesDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategory(id);
                var categoryDto = _mapper.Map<CategoryDTO>(category);

                if (category == null)
                {
                    return NotFound($"There is no category with the ID: {id}");
                }
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryDto.Name))
                {
                    return BadRequest("Category name is null");
                }
                var category = _mapper.Map<Category>(categoryDto);
                category.Active = true;
                var newCategory = await _categoryRepository.AddCategory(category);
                var newCategoryDto = _mapper.Map<CategoryDTO>(newCategory);
                return CreatedAtAction("GetCategory", new { id = newCategoryDto.Id }, newCategoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            try
            {
                if (id != categoryDto.Id || string.IsNullOrWhiteSpace(categoryDto.Name))
                {
                    return BadRequest("Some fields have incorrect values");
                }
                
                var existingCategory = await _categoryRepository.GetCategory(id);
                if (existingCategory == null)
                {
                    return NotFound($"There is no category with the ID: {id}");
                }
                if (existingCategory.Active == false)
                {
                    return BadRequest($"The category with ID: {id} is inactive");
                }

                var category = _mapper.Map<Category>(categoryDto);
                await _categoryRepository.UpdateCategory(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategory(id);
                if (category == null || category.Active == false)
                {
                    return NotFound($"There is no category with the ID: {id}");
                }
                await _categoryRepository.DeleteCategory(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
