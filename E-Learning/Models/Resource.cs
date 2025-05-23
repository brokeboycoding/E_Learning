using System;
using System.Collections.Generic;

namespace E_Learning.Models;

public partial class Resource
{
    public int Id { get; set; }

    public string? FileName { get; set; }

    public string FileUrl { get; set; } = null!;

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;
}
