using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.CalculatorBMI;

public partial class BmiResult
{
    public int Id { get; set; }

    public DateTime? CalculationDate { get; set; }

    public decimal? Result { get; set; }

    public string UserName { get; set; }
}