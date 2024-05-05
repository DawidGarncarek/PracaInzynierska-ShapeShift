using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Calories;

public partial class UserCaloriesResult
{
    public int Id { get; set; }

    public DateTime? NewCaloriesDate { get; set; }

    public int? ProductId { get; set; }

    public decimal? Calories { get; set; }

    public string UserName { get; set; }

    public virtual FoodProducts Product { get; set; }
}