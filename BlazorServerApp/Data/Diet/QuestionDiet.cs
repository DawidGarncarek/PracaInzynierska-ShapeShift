using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Diet;

public partial class QuestionDiet
{
    public int Id { get; set; }

    public string QuestionText { get; set; }

    public string Category { get; set; }

    public DateTime? DateCreated { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<PossibleAnswersDiet> PossibleAnswersDiet { get; set; } = new List<PossibleAnswersDiet>();

    public virtual ICollection<UserAnswersDiet> UserAnswersDiet { get; set; } = new List<UserAnswersDiet>();

    public virtual ICollection<UserAnswersDietCalories> UserAnswersDietCalories { get; set; } = new List<UserAnswersDietCalories>();
}