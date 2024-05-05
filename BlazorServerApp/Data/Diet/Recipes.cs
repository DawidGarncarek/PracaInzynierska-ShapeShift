using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Diet;

public partial class Recipes
{
    public int Id { get; set; }

    public int MealId { get; set; }

    public string RecipeText { get; set; }

    public virtual Meals Meal { get; set; }
}