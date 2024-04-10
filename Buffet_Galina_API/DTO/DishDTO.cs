using Galina;
using System.Text.Json.Serialization;

namespace Buffet_Galina_API.DTO
{
    public partial class DishDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public int Price { get; set; }

        public byte[]? Image { get; set; }
        //[JsonConverter(typeof(TimeOnlyJsonConvert))]
        public List<ProductDTO> Products { get; set; }
        
    }
}
