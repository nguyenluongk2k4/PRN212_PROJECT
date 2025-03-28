using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? Describe { get; set; }

    public string? Image { get; set; }

    public int? Cid { get; set; }

    public virtual Category? CidNavigation { get; set; }
}
