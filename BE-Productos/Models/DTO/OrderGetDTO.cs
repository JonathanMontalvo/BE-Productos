namespace BE_Productos.Models.DTO
{
    public class OrderGetDTO
    {
        public int Id { get; set; }
        public virtual EmployeeDTO Employee { get; set; } = null!;
        public virtual ICollection<OrdersProductInOrderDTO> OrdersProducts { get; set; } = new List<OrdersProductInOrderDTO>();
        public decimal Total { get; set; }
    }
}
