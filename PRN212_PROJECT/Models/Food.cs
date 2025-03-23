using System;
using System.Collections.Generic;

namespace PRN212_PROJECT.Models;

public partial class Food
{
    public int FoodId { get; set; }

    public string? FoodName { get; set; }

    public int? FoodType { get; set; }

    public double? Price { get; set; }

    public int? Table { get; set; }

    public int? Status { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<ComboDetail> ComboDetails { get; set; } = new List<ComboDetail>();

    public virtual TypeOfFood? FoodTypeNavigation { get; set; }

    public virtual ICollection<OrderDetailFood> OrderDetailFoods { get; set; } = new List<OrderDetailFood>();
}
