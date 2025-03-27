using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Expenditure
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Executor { get; set; }

    public decimal? Cost { get; set; }

    public int? SupplierOrderId { get; set; }

    public virtual SupplierOrder? SupplierOrder { get; set; }
}
