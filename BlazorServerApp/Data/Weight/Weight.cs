using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Weight;

public partial class Weight
{
    public int Id { get; set; }

    public DateTime? WeightDate { get; set; }

    public decimal? UserWeight { get; set; }

    public decimal? GoalWeight { get; set; }

    public string UserName { get; set; }
}