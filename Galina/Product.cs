using System;
using System.Collections.Generic;

namespace Galina;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<DishProduct> DishProducts { get; set; } = new List<DishProduct>();
}
