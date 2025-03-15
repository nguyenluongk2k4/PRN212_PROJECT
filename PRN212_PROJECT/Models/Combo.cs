using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Combo
{
    public int ComboId { get; set; }

    public string ComboName { get; set; } = null!;

    public int Status { get; set; }

    public double Price { get; set; }

    public virtual ICollection<ComboDetail> ComboDetails { get; set; } = new List<ComboDetail>();
}
