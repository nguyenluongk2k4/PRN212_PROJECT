using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class TypeOfFood
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
}
