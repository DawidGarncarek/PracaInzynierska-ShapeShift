using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Exercises;

public partial class UserExerciseSets
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public int ExerciseSetId { get; set; }

    public DateTime DateAssigned { get; set; }

    public int? TrainingDay { get; set; }

    public virtual Exercises ExerciseSet { get; set; }
}