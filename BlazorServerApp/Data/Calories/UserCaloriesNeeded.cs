using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Calories;

public partial class UserCaloriesNeeded
{
    public int Id { get; set; }

    public DateTime? NewCaloriesDate { get; set; }

    public decimal? CaloriesNeededResult { get; set; }

    public string UserName { get; set; }
}