using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Diet;

public partial class UserAnswersDietCalories
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public int Answer { get; set; }

    public DateTime? DateAnswered { get; set; }

    public string UserName { get; set; }

    public virtual QuestionDiet Question { get; set; }
}