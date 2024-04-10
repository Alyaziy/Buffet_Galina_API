using System;
using System.Collections.Generic;

namespace Galina;

public partial class Category
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Dish1> Dish1s { get; set; } = new List<Dish1>();
}
