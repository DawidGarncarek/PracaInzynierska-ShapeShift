using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.ContactForm;

public partial class ContactMessage
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public string Name { get; set; }

    public string EMail { get; set; }

    public string Topic { get; set; }

    public string Message { get; set; }
}