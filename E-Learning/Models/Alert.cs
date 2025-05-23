using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Alert
{
    public int Id { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsResolved { get; set; }
}
