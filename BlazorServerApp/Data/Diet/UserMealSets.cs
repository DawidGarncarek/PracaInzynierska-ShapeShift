using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Diet;

public partial class UserMealSets
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public int? BreakfastId { get; set; }

    public int? LunchId { get; set; }

    public int? DinnerId { get; set; }

    public int? MealDay { get; set; }

    public virtual Meals Breakfast { get; set; }

    public virtual Meals Dinner { get; set; }

    public virtual Meals Lunch { get; set; }
}