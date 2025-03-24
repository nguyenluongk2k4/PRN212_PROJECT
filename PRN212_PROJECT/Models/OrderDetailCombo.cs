using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class OrderDetailCombo
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ComboId { get; set; }

    public int? Amount { get; set; }

    public double? Price { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual OrderTable? Order { get; set; }
}
