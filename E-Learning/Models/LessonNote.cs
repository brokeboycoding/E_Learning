using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Lessonnote
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int? LessonId { get; set; }

    public TimeOnly? Timestamp { get; set; }

    public string? Content { get; set; }

    public virtual Lesson? Lesson { get; set; }

    public virtual Aspnetuser? User { get; set; }
}
