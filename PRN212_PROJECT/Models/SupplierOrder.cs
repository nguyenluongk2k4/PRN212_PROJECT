using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN212_PROJECT.Models;

public partial class SupplierOrder
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public DateOnly? DeliverDate { get; set; }

    public bool? IsPaid { get; set; }

    public decimal? Total { get; set; }
    [NotMapped]
    public bool IsSelected { get; set; }

    public virtual ICollection<Expenditure> Expenditures { get; set; } = new List<Expenditure>();

    public virtual Supplier Supplier { get; set; } = null!;
}
