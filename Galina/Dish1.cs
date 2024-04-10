using System;
using System.Collections.Generic;

namespace Galina;

public partial class Dish1
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int CategoryId { get; set; }

    public int Price { get; set; }

    public byte[]? Image { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<DishProduct> DishProducts { get; set; } = new List<DishProduct>();

    public virtual ICollection<OrderDish> OrderDishes { get; set; } = new List<OrderDish>();
}
