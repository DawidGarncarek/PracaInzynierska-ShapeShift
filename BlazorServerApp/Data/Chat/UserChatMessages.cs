using System;
using System.Collections.Generic;

namespace BlazorServerAppDB.Data.Chat;

public partial class UserChatMessages
{
    public int Id { get; set; }

    public string MessageText { get; set; }

    public DateTime? Date { get; set; }

    public string UserName { get; set; }
}