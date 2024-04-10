using System;
using System.Collections.Generic;

namespace Galina;

public partial class OrderDish
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int DishId { get; set; }

    public int Value { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Dish1 Dish { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
