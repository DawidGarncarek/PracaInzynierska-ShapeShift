using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Exercises;

public partial class Exercises
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string IntensityLevel { get; set; }

    public int? DurationMinutes { get; set; }

    public string Location { get; set; }

    public string Goals { get; set; }

    public string DifficultyLevel { get; set; }

    public virtual ICollection<UserExerciseSets> UserExerciseSets { get; set; } = new List<UserExerciseSets>();
}