using System;
using System.Collections.Generic;

namespace Galina;

public partial class Order
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderDish> OrderDishes { get; set; } = new List<OrderDish>();
}
