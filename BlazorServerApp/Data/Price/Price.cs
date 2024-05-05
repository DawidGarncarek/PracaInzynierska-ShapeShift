using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Price;

public partial class Price
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public decimal? Price1 { get; set; }

    public string Message { get; set; }

    public string UserName { get; set; }
}