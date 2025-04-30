using AutoMapper;
using BE_Productos.Models.DTO;

namespace BE_Productos.Models.Profiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile() {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
