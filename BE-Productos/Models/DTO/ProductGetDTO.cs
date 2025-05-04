namespace BE_Productos.Models.DTO
{
    public class ProductGetDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public virtual CategoryDTO Category { get; set; } = null!;
    }
}
