using System;
using System.Collections.Generic;

namespace Galina;

public partial class Admin
{
    public int Id { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
