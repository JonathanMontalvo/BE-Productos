namespace BE_Productos.Models.DTO
{
    public class OrdersProductGetDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual ProductDTO Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
