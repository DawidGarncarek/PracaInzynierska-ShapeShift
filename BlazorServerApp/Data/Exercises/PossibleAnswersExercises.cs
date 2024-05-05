using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Exercises;

public partial class PossibleAnswersExercises
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public string AnswerOption { get; set; }

    public string AnswerText { get; set; }

    public virtual QuestionExercises Question { get; set; }

    public virtual ICollection<UserAnswersExercises> UserAnswersExercises { get; set; } = new List<UserAnswersExercises>();
}