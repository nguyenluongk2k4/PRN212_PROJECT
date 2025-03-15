using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? FoodId { get; set; }

    public int Amount { get; set; }

    public double TotalCost { get; set; }

    public virtual Food? Food { get; set; }

    public virtual Order? Order { get; set; }
}
