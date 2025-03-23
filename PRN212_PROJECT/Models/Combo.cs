using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Combo
{
    public int ComboId { get; set; }

    public string? ComboName { get; set; }

    public int? Status { get; set; }

    public double? Price { get; set; }

    public virtual ICollection<ComboDetail> ComboDetails { get; set; } = new List<ComboDetail>();

    public virtual ICollection<OrderDetailCombo> OrderDetailCombos { get; set; } = new List<OrderDetailCombo>();
}
