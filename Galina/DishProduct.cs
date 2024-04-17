using System;
using System.Collections.Generic;

namespace Galina;

public partial class DishProduct
{
    public int Id { get; set; }

    public int? DishId { get; set; }

    public int ProductId { get; set; }

    public DateTime? CreatedAt { get; set; } 

    public DateTime? UpdatedAt { get; set; }

    public virtual Dish1? Dish { get; set; }

    public virtual Product Product { get; set; } = null!;
}
