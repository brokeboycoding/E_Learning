using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Lessonprogress
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int LessonId { get; set; }

    public bool IsCompleted { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;
}
