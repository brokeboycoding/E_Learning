using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Module
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Count { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
