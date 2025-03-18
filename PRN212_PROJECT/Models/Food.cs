using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Food
{
    public int FoodId { get; set; }

    public string FoodName { get; set; } = null!;

    public int? FoodType { get; set; }
    //public string TypeName { get; set; }

    public double Price { get; set; }

    public int Status { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<ComboDetail> ComboDetails { get; set; } = new List<ComboDetail>();

    public virtual TypeOfFood? FoodTypeNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

}
