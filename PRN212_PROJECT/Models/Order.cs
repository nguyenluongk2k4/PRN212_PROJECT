using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = null!;

    public DateTime Date { get; set; }

    public bool IsPaid { get; set; }

    public string Address { get; set; } = null!;

    public bool Done { get; set; }

    public double? Total { get; set; }

    //public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
