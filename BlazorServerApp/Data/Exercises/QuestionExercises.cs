using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Exercises;

public partial class QuestionExercises
{
    public int Id { get; set; }

    public string QuestionText { get; set; }

    public string Category { get; set; }

    public DateTime? DateCreated { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<PossibleAnswersExercises> PossibleAnswersExercises { get; set; } = new List<PossibleAnswersExercises>();

    public virtual ICollection<UserAnswersExercises> UserAnswersExercises { get; set; } = new List<UserAnswersExercises>();
}