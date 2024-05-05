using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Diet;

public partial class PossibleAnswersDiet
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public string AnswerOption { get; set; }

    public string AnswerText { get; set; }

    public virtual QuestionDiet Question { get; set; }

    public virtual ICollection<UserAnswersDiet> UserAnswersDiet { get; set; } = new List<UserAnswersDiet>();
}