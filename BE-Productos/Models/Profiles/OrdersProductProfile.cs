using AutoMapper;
using BE_Productos.Models.DTO;

namespace BE_Productos.Models.Profiles
{
    public class OrdersProductProfile: Profile
    {
        public OrdersProductProfile()
        {
            //Para evitar errores por la coversión de Product a ProductDTO debemos de tener en el Profile de Product su Mapping
            CreateMap<OrdersProduct, OrdersProductGetDTO>();

            //Utilizado para mostrar en Orders
            CreateMap < OrdersProduct, OrdersProductInOrderDTO>();

        }
    }
}
