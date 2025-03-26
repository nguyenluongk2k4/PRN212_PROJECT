using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class SupplierOrder
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public DateOnly? DeliverDate { get; set; }

    public int? Status { get; set; }

    public bool? IsPaid { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<Expenditure> Expenditures { get; set; } = new List<Expenditure>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; } = new List<SupplierOrderDetail>();
}
