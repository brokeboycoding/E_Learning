using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Discussion
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string UserId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual Aspnetuser User { get; set; } = null!;
}
