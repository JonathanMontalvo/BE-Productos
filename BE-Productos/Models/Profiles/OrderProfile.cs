using AutoMapper;
using BE_Productos.Models.DTO;

namespace BE_Productos.Models.Profiles
{
    public class OrderProfile:Profile
    {
        /*
         * Para evitar errores por la conversión de la propiedad OrdersProducts de nuestro DTO como tenemos OrdersProductInOrderDTO 
         * y en el modelo el ICollection es OrdersProduct, se le indica a AutoMapper que la propiedad OrdersProducts del DTO es 
         * igual a la propiedad OrdersProducts del modelo.
         * Pero aun así debemos de crear un Profile para que AutoMapper sepa que existe una relación entre el modelo y el DTO.
         */
        public OrderProfile() {
            CreateMap<Order, OrderGetDTO>()
                .ForMember(destiny => destiny.OrdersProducts, options => options.MapFrom(source => source.OrdersProducts));
        }
    }
}
