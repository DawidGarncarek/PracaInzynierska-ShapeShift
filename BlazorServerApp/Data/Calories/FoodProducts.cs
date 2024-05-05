using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Calories;

public partial class FoodProducts
{
    public int Id { get; set; }

    public string ProductName { get; set; }

    public decimal Protein { get; set; }

    public decimal Fats { get; set; }

    public decimal Carbohydrates { get; set; }

    public decimal Calories100g { get; set; }

    public virtual ICollection<UserCaloriesResult> UserCaloriesResult { get; set; } = new List<UserCaloriesResult>();
}