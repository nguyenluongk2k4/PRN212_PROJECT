using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class SupplierOrderDetail
{
    public int Id { get; set; }

    public int SupplierOrderId { get; set; }

    public string? ProductName { get; set; }

    public double? Amount { get; set; }

    public double? UnitPrice { get; set; }

    public string? CalculationUnit { get; set; }
}
