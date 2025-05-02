namespace BE_Productos.Models.DTO
{
    public class OrdersProductInOrderDTO
    {
        public int Id { get; set; }
        public virtual ProductDTO Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
