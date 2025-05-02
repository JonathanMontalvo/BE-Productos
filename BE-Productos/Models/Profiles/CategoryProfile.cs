using AutoMapper;
using BE_Productos.Models.DTO;

namespace BE_Productos.Models.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
        }
    }
}
