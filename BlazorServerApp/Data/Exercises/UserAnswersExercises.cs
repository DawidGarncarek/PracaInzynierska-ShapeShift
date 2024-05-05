using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Exercises;

public partial class UserAnswersExercises
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public int ChosenAnswerId { get; set; }

    public DateTime? DateAnswered { get; set; }

    public string UserName { get; set; }

    public virtual PossibleAnswersExercises ChosenAnswer { get; set; }

    public virtual QuestionExercises Question { get; set; }
}