namespace Buffet_Galina_API.DTO
{
    public partial class DishProductDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int DishId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public ProductDTO Product { get; internal set; }
    }
}
