using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class OrderTable
{
    public int OrderId { get; set; }

    public string? CustomerName { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsPaid { get; set; }

    public bool? Done { get; set; }

    public double? Total { get; set; }

    public string? Address { get; set; }

    public bool? Shipping { get; set; }

    public virtual ICollection<OrderDetailCombo> OrderDetailCombos { get; set; } = new List<OrderDetailCombo>();

    public virtual ICollection<OrderDetailFood> OrderDetailFoods { get; set; } = new List<OrderDetailFood>();
}
