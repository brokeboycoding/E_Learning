using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? VideoUrl { get; set; }

    public string? Description { get; set; }

    public int ModuleId { get; set; }

    public virtual ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();

    public virtual ICollection<Lessonnote> Lessonnotes { get; set; } = new List<Lessonnote>();

    public virtual ICollection<Lessonprogress> Lessonprogresses { get; set; } = new List<Lessonprogress>();

    public virtual Module Module { get; set; } = null!;
}
