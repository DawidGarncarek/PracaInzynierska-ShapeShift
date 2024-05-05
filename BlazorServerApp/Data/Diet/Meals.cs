using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Diet;

public partial class Meals
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Protein { get; set; }

    public decimal Fats { get; set; }

    public decimal Carbohydrates { get; set; }

    public int Calories { get; set; }

    public string MealType { get; set; }

    public string Goals { get; set; }

    public string Allergy { get; set; }

    public string Type { get; set; }

    public string FoodType { get; set; }

    public string ProductType { get; set; }

    public string RegionTypeFood { get; set; }

    public string ExcludedProducts { get; set; }

    public virtual ICollection<Recipes> Recipes { get; set; } = new List<Recipes>();

    public virtual ICollection<UserMealSets> UserMealSetsBreakfast { get; set; } = new List<UserMealSets>();

    public virtual ICollection<UserMealSets> UserMealSetsDinner { get; set; } = new List<UserMealSets>();

    public virtual ICollection<UserMealSets> UserMealSetsLunch { get; set; } = new List<UserMealSets>();
}