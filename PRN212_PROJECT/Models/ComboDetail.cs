using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class ComboDetail
{
    public int ComboDetailId { get; set; }

    public int? FoodId { get; set; }

    public int Amount { get; set; }

    public int? ComboId { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Food? Food { get; set; }
}
