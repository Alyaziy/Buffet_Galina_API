namespace Buffet_Galina_API.DTO
{
    public partial class OrderDTO
    {
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<DishDTO> DishDTOs { get; set; }
    }
}
