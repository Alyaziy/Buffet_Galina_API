namespace Buffet_Galina_API.DTO
{
    public partial class CategoryDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
