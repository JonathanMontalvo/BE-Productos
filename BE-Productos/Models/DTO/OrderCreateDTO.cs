namespace BE_Productos.Models.DTO
{
    public class OrderCreateDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public List<OrdersProductCreateDTO> OrdersProducts { get; set; } = new List<OrdersProductCreateDTO>();
        public decimal Total { get; set; }
        
    }
}
